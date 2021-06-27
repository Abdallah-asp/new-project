using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.BookVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IBook : IService
    {
        IQueryable<GetBookViewModel> GetBooks(UserParam param);
        Task<BookDetailsViewModel> BookDetails(int id, UserParam param);
        Task<GetBookAndHisFeedBackViewModel> GetBookDetailsById(int id);
        Task<SaveBookViewModel> GetUpdatedBookById(int id);
        Task<Book> GetBook(int id);
        string GetBookAuthorId(int id);
        int GetBookIdByName(string name);
        Task<bool> IsBookExistBefore(string name);
        Task<string> SaveBook(SaveBookViewModel model, string root);
        Task<List<GetBooksDropDownListViewModel>> GetBooksDropDownList();
        Task<PagedList<GetBookViewModel>> GetBooksWithPadination(UserParam param);
    }
}
