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

        public BorrowTheBookService(DataContext context,
            ICoreBase repoCore,
            IUser repoUser,
            IBook repoBook)
        {
            _context = context;
            _repoCore = repoCore;
            _repoUser = repoUser;
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

        public async Task<string> SaveBorrowTheBook(SaveBorrowTheBookViewModel model)
        {
            BorrowTheBook borrow = new BorrowTheBook
            {
                Date = DateTime.UtcNow,
                Borrowing_period_date = model.Borrowing_period_date,
                Actual_return_date = model.Actual_return_date,
                User_id = _repoUser.GetUserIdByName(model.User_name),
                College_id = model.College_id.Value,
                Book_id = _repoBook.GetBookIdByName(model.Book_name)
            };

            _repoCore.Add(borrow);
            await _repoCore.SaveAll();
            
            return null;
        }
    }
}
