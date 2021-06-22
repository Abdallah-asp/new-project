using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _repo;
        private readonly UserManager<User> _userManager;
        private readonly ICoreBase _repoCore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _webHost;

        public UserController(ICoreBase repoCore,
                               IUser repo,
                               UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IWebHostEnvironment webHost)
        {
            _repo = repo;
            _userManager = userManager;
            _repoCore = repoCore;
            _roleManager = roleManager;
            _webHost = webHost;
        }

        public IActionResult Login()
        {
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role);

            if (identity != null)
            {
                if (role != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Password))
            {
                model.Error = "check all feilds not empty";
                return View(model);
            }

            var res = await _repo.Login(model);

            if (res != null)
            {
                HttpContext.Session.SetString("JWToken", new JwtSecurityTokenHandler().WriteToken(res.Token));
                return RedirectToAction("Index","Home");
            }

            model.Error = "name or password is wrong";
            return View(model);
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult GetAuthors()
        {
            return View();
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult GetDoctors()
        {
            return View();
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult GetStudents()
        {
            return View();
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult GetAdmins()
        {
            return View();
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult Add()
        {
            var model = new SaveUserViewModel();

            return View("UserForm", model);
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost]
        public async Task<IActionResult> Save(SaveUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserForm", model);
            }

            if (await _roleManager.RoleExistsAsync(model.Role) == false)
            {
                model.Error = new List<string> { "Role is wrong or not exist" };

                return View("UserForm", model);
            }

            if (await _repo.IsEmailExistBefore(model.Email))
            {
                model.Error = new List<string> { "Email is used before" };

                return View("UserForm", model);
            }

            if (_roleManager.Roles.Count() == 0)
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "Author", NormalizedName = "AUTHOR"},
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN"},
                    new IdentityRole { Name = "Doctor", NormalizedName = "DOCTOR"},
                    new IdentityRole { Name = "Student", NormalizedName = "Student"}
                };

                foreach (var item in roles)
                {
                    await _roleManager.CreateAsync(item);
                }
            }
            var root = Path.Combine(_webHost.WebRootPath, "upload");

            var user = await _repo.SaveUser(model, root);

            if (await _userManager.FindByIdAsync(user.Id) != null)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            else
            {
                return View("UserForm", model);
            }

            if (model.Role == "Admin")
            {
                return RedirectToAction(nameof(GetAdmins));
            }

            return RedirectToAction(nameof(GetAuthors));
            
        }

        [Authorize(Policy = "AdminRequire")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }
    }
}
