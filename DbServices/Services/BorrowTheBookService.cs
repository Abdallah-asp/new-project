using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.BorrowTheBookVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class BorrowTheBookService : BaseService , IBorrowTheBook
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IUser _repoUser;
        private IBook _repoBook;
        private ICollege _repoCollege;

        public BorrowTheBookService(DataContext context,
            ICoreBase repoCore,
            IUser repoUser,
            ICollege repoCollege,
            IBook repoBook)
        {
            _context = context;
            _repoCore = repoCore;
            _repoUser = repoUser;
            _repoCollege = repoCollege;
            _repoBook = repoBook;
        }

        public async Task<PagedList<GetBorrowTheBookViewModel>> GetBorrowTheBooksWithPadination(UserParam param)
        {
            var borrowTheBooks = _context.BorrowTheBooks
                .Include(c => c.Book)
                .Include(c => c.User)
                .Include(c => c.College)
                .Select(c => new GetBorrowTheBookViewModel
                {
                    Id = c.Id,
                    Date = c.Date.ToString("yyyy-MM-dd"),
                    Borrowing_period_date = c.Borrowing_period_date.ToString("yyyy-MM-dd"),
                    Actual_return_date = c.Actual_return_date.ToString("yyyy-MM-dd"),
                    User_name = c.User.UserName,
                    Book_name = c.Book.Name_en,
                    College_name = c.College.Name_en
                    
                }).AsQueryable();


            if (!string.IsNullOrEmpty(param.Key))
            {
                borrowTheBooks = borrowTheBooks.Where(c => c.User_name.Contains(param.Key) 
                    || c.Book_name.Contains(param.Key) 
                    || c.College_name.Contains(param.Key));
            }

            return await PagedList<GetBorrowTheBookViewModel>
                .CreateAsync(borrowTheBooks, param.PageNumber, param.PageSize);
        }

        public async Task<SaveBorrowTheBookViewModel> GetUpdatedBorrowDataById(int id)
        {
            var borrowData = await _context.BorrowTheBooks
                .FirstOrDefaultAsync(b => b.Id == id);

            var model = new SaveBorrowTheBookViewModel
            {
                Id = borrowData.Id,
                Date = borrowData.Date,
                Borrowing_period_date = borrowData.Borrowing_period_date,
                Actual_return_date = borrowData.Actual_return_date,
                Colleges = await _repoCollege.GetCollegesDrobDownList(),
                Books = await _repoBook.GetBooksDropDownList(),
                Users = await _repoUser.GetUserDrobDownList(),
                College_id = borrowData.College_id,
                User_id = borrowData.User_id,
                Book_id = borrowData.Book_id
            };

            return model;
        }

        public async Task<string> SaveBorrowTheBook(SaveBorrowTheBookViewModel model)
        {
            if (model.Id == 0)
            {
                BorrowTheBook borrow = new BorrowTheBook
                {
                    Date = DateTime.UtcNow,
                    Borrowing_period_date = model.Borrowing_period_date,
                    Actual_return_date = model.Actual_return_date,
                    User_id = model.User_id,
                    College_id = model.College_id.Value,
                    Book_id = model.Book_id.Value
                };

                _repoCore.Add(borrow);
            }
            else
            {
                var borrowData = await _context.BorrowTheBooks.FindAsync(model.Id);

                borrowData.Date = model.Date;
                borrowData.Borrowing_period_date = model.Borrowing_period_date;
                borrowData.Actual_return_date = model.Actual_return_date;
            }

            await _repoCore.SaveAll();

            return null;
        }
    }
}
