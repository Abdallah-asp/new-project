using Interfaces.Interfaces;
using Interfaces.ViewModel.CategoryVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    [Authorize(Policy = "AdminRequire")]
    public class CategoryController : Controller
    {
        private ICategory _repo;
        private ICoreBase _repoCore;

        public CategoryController(ICategory repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new SaveCategoryViewModel();

            return View("CategoryForm", model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _repo.GetUpdatedCategoryById(id);

            return View("CategoryForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SaveCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryForm", model);
            }

            var result = await _repo.SaveCategory(model);
            await _repoCore.SaveAll();

            if (result != null)
            {
                model.Error = result;
                return View("CategoryForm", model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
