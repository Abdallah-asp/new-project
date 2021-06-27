using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.UserViewModel;
using Interfaces.ViewModel.UserVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryApp.APi
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _repo;
        private readonly UserManager<User> _userManager;
        private readonly ICoreBase _repoCore;
        private readonly ISms _repoSms;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _webHost;

        public UsersController(ICoreBase repoCore,
                               IUser repo,
                               ISms repoSms,
                               UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IWebHostEnvironment webHost)
        {
            _repo = repo;
            _userManager = userManager;
            _repoCore = repoCore;
            _repoSms = repoSms;
            _roleManager = roleManager;
            _webHost = webHost;
        }

        [Authorize]
        [HttpPost("edit")]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var root = Path.Combine(_webHost.WebRootPath, "upload");

            if (model.Image != string.Empty)
            {
                string fileName;

                var isImageSaved = _repoCore.SaveSingleImageBase64(root, model.Image, out fileName);

                if (!isImageSaved)
                {
                    return BadRequest(new { messageError = 10 });
                }

                model.Image = fileName;
            }

            var result = await _repo.EditProfile(id, model, root);

            if (result != null)
            {
                return Ok(new { messageSuccess = 1 });
            }

            return BadRequest(new { messageError = 9 });
        }

        [Authorize]
        [Route("profile/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProfile([FromRoute]string id,[FromQuery] UserParam param)
        {
            var root = Path.Combine(_webHost.WebRootPath, "upload");
            var profile = await _repo.GetProfile(id, param, root);

            return Ok(profile);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(new { messageError = 3});
            }

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new { messageError = 4 });
            }

            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { messageError = 5 });
            }

            if (string.IsNullOrEmpty(model.Role) || string.IsNullOrWhiteSpace(model.Role))
            {
                return BadRequest(new { messageError = 6 });
            }

            if (await _repo.IsEmailExistBefore(model.Email))
            {
                return BadRequest(new { messageError = 9 });
            }

            model.Image = Path.Combine(_webHost.WebRootPath, "images/avatar/avatar.png");
            var result = await _repo.Register(model);

            if (result)
            {
                return Ok(new { messageSuccess = 1 });
            }

            return BadRequest(new { messageError = 7 });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(new { messageError = 3 });
            }

            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { messageError = 5 });
            }

            var result = await _repo.Login(model);

            if (result != null)
            {
                return Ok(new { 
                    messageSuccess = 1,
                    token = new JwtSecurityTokenHandler().WriteToken(result.Token) 
                });
            }

            return BadRequest(new { messageError = 8 });
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost("getAuthors")]
        public async Task<IActionResult> GetAuthorsWithPagiation()
        {
            try
            {
                // Datatable Pagination Server Side Properties
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int PageNumber = (int.Parse(start) / pageSize);
                // Datatable Properties End

                var users = await _repo.GetUsersWithPagination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue,
                    Role = "Author"
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = users.TotalCount,
                    recordsTotal = users.TotalCount,
                    data = users
                };
                // Send to View End

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost("getDoctors")]
        public async Task<IActionResult> GetDoctorsWithPagiation()
        {
            try
            {
                // Datatable Pagination Server Side Properties
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int PageNumber = (int.Parse(start) / pageSize);
                // Datatable Properties End

                var users = await _repo.GetUsersWithPagination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue,
                    Role = "Doctor"
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = users.TotalCount,
                    recordsTotal = users.TotalCount,
                    data = users
                };
                // Send to View End

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost("getStudents")]
        public async Task<IActionResult> GetStudentsWithPagiation()
        {
            try
            {
                // Datatable Pagination Server Side Properties
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int PageNumber = (int.Parse(start) / pageSize);
                // Datatable Properties End

                var users = await _repo.GetUsersWithPagination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue,
                    Role = "Student"
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = users.TotalCount,
                    recordsTotal = users.TotalCount,
                    data = users
                };
                // Send to View End

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost("getAdmins")]
        public async Task<IActionResult> GetAdminsWithPagiation()
        {
            try
            {
                // Datatable Pagination Server Side Properties
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int PageNumber = (int.Parse(start) / pageSize);
                // Datatable Properties End

                var users = await _repo.GetUsersWithPagination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue,
                    Role = "Admin",
                    UserCurrentId = User.FindFirst(ClaimTypes.NameIdentifier).Value
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = users.TotalCount,
                    recordsTotal = users.TotalCount,
                    data = users
                };
                // Send to View End

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("changePasword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Old_password) || string.IsNullOrWhiteSpace(model.Old_password))
                {
                    return BadRequest(new { messageError = 11 });
                }

                if (string.IsNullOrEmpty(model.New_password) || string.IsNullOrWhiteSpace(model.New_password))
                {
                    return BadRequest(new { messageError = 12 });
                }

                if (string.IsNullOrEmpty(model.Confirm_password) || string.IsNullOrWhiteSpace(model.Confirm_password))
                {
                    return BadRequest(new { messageError = 13 });
                }

                var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var checkPassword = await _userManager.CheckPasswordAsync(user, model.Old_password);

                if (!checkPassword)
                {
                    return BadRequest(new { messageError = 14 });
                }

                if (model.New_password != model.Confirm_password)
                {
                    return BadRequest(new { messageError = 15 });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.New_password);

                if (!result.Succeeded)
                {
                    return BadRequest(new { messageError = 16 });
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                JwtSecurityToken newToken = _repo.GenerateToken(userRoles, user);

                return Ok(new { messageSuccess = 1 });
            }
            catch (Exception e)
            {
                return BadRequest(new { messageError = 21 });
            }
        }

        [HttpPost("sendConfirmationCode")]
        public async Task<IActionResult> SendConfirmationCode([FromBody] PhoneRequestViewModel model)
        {
            try
            {
                var user = await _repo.GetUserByPhoneNumber(model.Phone);

                if (user == null)
                {
                    return BadRequest(new { messageError = 17 });
                }

                user.Confirmation_code = _repoCore.GenerateRandomCodeAsNumber();

                await _repoCore.SaveAll();

                //var body = " Confirmation code is" + " " + user.Confirmation_code;
                //var phone = model.CountryCode + model.Phone;
                //await _repoSms.SendMessage(phone, body);

                return Ok(new { messageSuccess = 1 });
            }
            catch (Exception e)
            {
                return BadRequest(new { messageError = 18 });
            }
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateConfirmationCode([FromBody] PhoneRequestViewModel model)
        {
            try
            {
                var user = await _repo.GetUserByPhoneNumber(model.Phone);

                if (user == null)
                {
                    return BadRequest(new { messageError = 17 });
                }

                if (user.Confirmation_code != model.Code)
                {
                    return BadRequest(new { messageError = 19 });
                }

                return Ok(new { messageSuccess = 1 });
            }
            catch (Exception e)
            {
                return BadRequest(new { messageError = 20 });
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PhoneRequestViewModel model)
        {
            try
            {
                var user = await _repo.GetUserByPhoneNumber(model.Phone);

                if (user == null)
                {
                    return BadRequest(new { messageError = 17 });
                }

                if (string.IsNullOrEmpty(model.New_password) || string.IsNullOrWhiteSpace(model.New_password))
                {
                    return BadRequest(new { messageError = 12 });
                }

                if (string.IsNullOrEmpty(model.Confirm_password) || string.IsNullOrWhiteSpace(model.Confirm_password))
                {
                    return BadRequest(new { messageError = 13 });
                }

                if (model.New_password != model.Confirm_password)
                {
                    return BadRequest(new { messageError = 15 });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.New_password);

                if (!result.Succeeded)
                {
                    return BadRequest(new { messageError = 16 });
                }

                return Ok(new { messageSuccess = 1 });
            }
            catch (Exception e)
            {
                return BadRequest(new { messageError = 21 });
            }
        }
    }
}
