using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.Interfaces;
using Interfaces.ViewModel.CollegeVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Authorize(Policy = "AdminRequire")]
    public class CollegeController : Controller
    {
        private ICollege _repo;
        private ICoreBase _repoCore;

        public CollegeController(ICollege repo,
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
            var model = new SaveCollegeViewModel();

            return View("CollegeForm", model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _repo.GetUpdatedCollegeById(id);

            return View("CollegeForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SaveCollegeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CollegeForm", model);
            }

            var result = await _repo.SaveCollege(model);
            await _repoCore.SaveAll();

            if (result != null)
            {
                model.Error = result;
                return View("CollegeForm", model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
