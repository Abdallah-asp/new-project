using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.BorrowTheBookVM;
using Interfaces.ViewModel.UserViewModel;
using Interfaces.ViewModel.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class UserService : BaseService, IUser
    {
        private ICoreBase _repoCore;
        private DataContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public UserService(ICoreBase repoCore,
            DataContext context,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _repoCore = repoCore;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<User> SaveUser(SaveUserViewModel model, string root)
        {
            var emailSperated = model.Email.Split('@');
            var username = emailSperated[0] + _repoCore.GenerateRandomCodeAsNumber();

            if (model.File == null)
            {
                model.Image = null;
            }
            else
            {
                string fileName;
                var saveImage = _repoCore.SaveSingleImage(root, model.File, out fileName);

                model.Image = fileName;
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = username,
                Image = model.Image
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                model.Error = result.Errors.Select(e => e.Description).ToList();
            }

            return user;
        }

        public JwtSecurityToken GenerateToken(IList<string> usersRole, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in usersRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey:Key"]));

            var token =new JwtSecurityToken(
                issuer: "",
                audience: "",
                expires: DateTime.Now.AddYears(1),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return token;
        }

        public string GetRoleIdByName(string name)
        {
            var roleId = _context.Roles.FirstOrDefault(r => r.Name == name).Id;

            return roleId;
        }

        public string GetUserIdByName(string Name)
        {
            var userId = _context.Users.FirstOrDefault(u => u.Name == Name).Id;

            return userId;
        }

        public string GetUserNameById(string id)
        {
            var userName = _context.Users.FirstOrDefault(u => u.Id == id).Name;

            return userName;
        }

        public async Task<PagedList<GetUserViewModel>> GetUsersWithPagination(UserParam param)
        {
            var users = _context.Users.Where(c => _context.UserRoles
                .Any(x => x.RoleId == GetRoleIdByName(param.Role) && x.UserId == c.Id))
                .Select(c => new GetUserViewModel { 
                    User_name = c.UserName,
                    Name = c.Name,
                    Image = c.Image,
                    Email = c.Email
                });


            if (!string.IsNullOrEmpty(param.Key))
            {
                users = users.Where(c => c.Name.Contains(param.Key) 
                    || c.User_name.Contains(param.Key) 
                    || c.Email.Contains(param.Key));
            }

            return await PagedList<GetUserViewModel>
                .CreateAsync(users, param.PageNumber, param.PageSize);
        }

        public async Task<bool> IsEmailExistBefore(string email)
        {
            var isEmailInDb = await _context.Users.AnyAsync(e => e.Email == email);

            return isEmailInDb;
        }

        public async Task<bool> IsUserExistInDb(string name)
        {
            var isUserInDb = await _context.Users.AnyAsync(u => u.Name == name);

            return isUserInDb;
        }

        public async Task<LoginResultViewModel> Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == model.Name);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);

                GenerateToken(roles, user);

                return new LoginResultViewModel
                {
                    Token = GenerateToken(roles, user)
                };
            }

            return null;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var emailSperated = model.Email.Split('@');
            var username = emailSperated[0] + _repoCore.GenerateRandomCodeAsNumber();

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = username,
                Image = model.Image
            };

            var create = await _userManager.CreateAsync(user, model.Password);

            if (create.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<GetProfileViewModel> GetProfile(string id, UserParam param, string root)
        {
            var profile = await _context.Users
                .Include(u => u.Borrow_the_books)
                .Where(b => b.Id == id)
                .Select(c=> new GetProfileViewModel() { 
                    Email = c.Email,
                    Image_Url = root + c.Image,
                    Name = c.Name,
                    UserName = c.UserName,
                    Borrows = c.Borrow_the_books
                        .Select(b => new GetBorrowTheBookViewModel { 
                            Book_name = param.Lang == "en"? b.Book.Name_en : b.Book.Name_ar,
                            College_name = param.Lang == "en" ? b.College.Name_en : b.College.Name_ar,
                            Date = b.Date.ToString("yyyy-MM-dd"),
                            Borrowing_period_date = b.Borrowing_period_date.ToString("yyyy-MM-dd")
                        }).ToList()
                })
                .FirstOrDefaultAsync();


            return profile;
        }

        public async Task<EditProfileViewModel> EditProfile(string userId, EditProfileViewModel model, string root)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (model.Image != string.Empty)
            {
                string fullPath = root + user.Image;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Image = model.Image;

            await _repoCore.SaveAll();

            return new EditProfileViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Image = user.Image
            };
        }

        public async Task<List<UserDrobDownListViewModel>> GetUserDrobDownList(string role = null)
        {
            var users = new List<UserDrobDownListViewModel>();

            if (role == null)
            {
                users = await _context.Users.Where(u => _context.UserRoles
                .Any(r => r.RoleId != GetRoleIdByName("author") && r.UserId == u.Id))
                    .Select(u => new UserDrobDownListViewModel
                    {
                        Id = u.Id,
                        Name = u.Name
                    }).ToListAsync();
            }
            else
            {
                users = await _context.Users.Where(u => _context.UserRoles
                .Any(r => r.RoleId == GetRoleIdByName(role) && r.UserId == u.Id))
                    .Select(u => new UserDrobDownListViewModel
                    {
                        Id = u.Id,
                        Name = u.Name
                    }).ToListAsync();
            }

            return users;
        }

        public async Task<User> GetUserByPhoneNumber(string phone)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);

            return user;
        }
    }
}
