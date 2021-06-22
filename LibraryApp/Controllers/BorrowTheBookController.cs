using Interfaces.Interfaces;
using Interfaces.ViewModel.BorrowTheBookVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    [Authorize(Policy = "AdminRequire")]
    public class BorrowTheBookController : Controller
    {
        private IBorrowTheBook _repo;
        private ICollege _repoCollege;
        private IUser _repoUser;
        private IBook _repoBook;
        private ICoreBase _repoCore;

        public BorrowTheBookController(IBorrowTheBook repo,
            ICollege repoCollege,
            IBook repoBook,
            IUser repoUser,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCollege = repoCollege;
            _repoBook = repoBook;
            _repoUser = repoUser;
            _repoCore = repoCore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add()
        {
            var model = new SaveBorrowTheBookViewModel
            {
                Colleges = await _repoCollege.GetCollegesDrobDownList(),
                Books = await _repoBook.GetBooksDropDownList(),
                Users = await _repoUser.GetUserDrobDownList()
            };

            return View("BorrowForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SaveBorrowTheBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = new SaveBorrowTheBookViewModel
                {
                    Colleges = await _repoCollege.GetCollegesDrobDownList(),
                    Books = await _repoBook.GetBooksDropDownList(),
                    Users = await _repoUser.GetUserDrobDownList()
                };

                return View("BorrowForm", model);
            }

            var result = await _repo.SaveBorrowTheBook(model);

            if (result != null)
            {
                model.Error = result;

                model = new SaveBorrowTheBookViewModel
                {
                    Colleges = await _repoCollege.GetCollegesDrobDownList(),
                    Books = await _repoBook.GetBooksDropDownList(),
                    Users = await _repoUser.GetUserDrobDownList()
                };

                return View("BorrowForm", model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
