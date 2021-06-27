using Interfaces.Interfaces;
using Interfaces.ViewModel.PublishingHouseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    [Authorize(Policy = "AdminRequire")]
    public class PublishingHouseController : Controller
    {
        private IPublishingHouse _repo;
        private ICoreBase _repoCore;

        public PublishingHouseController(IPublishingHouse repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _repo.GetPublishingHouseDetails(id);

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new SavePublishingHouseViewModel();

            return View("PublishingHouseForm", model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _repo.GetUpdatedPublishingHouseById(id);

            return View("PublishingHouseForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SavePublishingHouseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("PublishingHouseForm", model);
            }

            var result = await _repo.SavePublishingHouse(model);
            await _repoCore.SaveAll();

            if (result != null)
            {
                model.Error = result;
                return View("PublishingHouseForm", model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
