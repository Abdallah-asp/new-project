using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.BookVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    [Authorize(Policy = "AdminRequire")]
    public class BookController : Controller
    {
        private IBook _repo;
        private ICategory _repoCategory;
        private IUser _repoUser;
        private IPublishingHouse _repoPublishingHouse;
        private ICoreBase _repoCore;
        private IWebHostEnvironment _webHost;

        public BookController(IBook repo,
            IUser repoUser,
            ICategory repoCategory,
            IPublishingHouse repoPublishingHouse,
            ICoreBase repoCore,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoCategory = repoCategory;
            _repoPublishingHouse = repoPublishingHouse;
            _repoCore = repoCore;
            _webHost = webHost;
            _repoUser = repoUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _repo.GetBookDetailsById(id);

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new SaveBookViewModel 
            {
                Categories = await _repoCategory.GetCategoriesDropDownList(),
                PublishingHouses = await _repoPublishingHouse.GetPublishingHouses(),
                users = await _repoUser.GetUserDrobDownList()
            };

            return View("BookForm", model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _repo.GetUpdatedBookById(id);

            return View("BookForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SaveBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = new SaveBookViewModel
                {
                    Categories = await _repoCategory.GetCategoriesDropDownList(),
                    PublishingHouses = await _repoPublishingHouse.GetPublishingHouses()
                };

                return View("BookForm", model);
            }

            var root = Path.Combine(_webHost.WebRootPath, "upload");

            var result = await _repo.SaveBook(model, root);
            await _repoCore.SaveAll();

            if (result != null)
            {
                model = new SaveBookViewModel
                {
                    Categories = await _repoCategory.GetCategoriesDropDownList(),
                    PublishingHouses = await _repoPublishingHouse.GetPublishingHouses()
                };

                model.Error = result;
                return View("BookForm", model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
