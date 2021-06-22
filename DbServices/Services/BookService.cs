using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.BookVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class BookService : BaseService , IBook
    {
        private DataContext _context;
        private ICategory _repoCategory;
        private IFeedBack _repoFeedBack;
        private IPublishingHouse _repoPhblishingHouse;
        private IUser _repoUser;
        private ICoreBase _repoCore;

        public BookService(DataContext context,
            ICategory repoCategory,
            IFeedBack repoFeedBack,
            IUser repoUser,
            IPublishingHouse repoPhblishingHouse,
            ICoreBase repoCore)
        {
            _context = context;
            _repoCategory = repoCategory;
            _repoFeedBack = repoFeedBack;
            _repoUser = repoUser;
            _repoPhblishingHouse = repoPhblishingHouse;
            _repoCore = repoCore;
        }

        public async Task<PagedList<GetBookViewModel>> GetBooksWithPadination(UserParam param)
        {
            var books = _context.Books
                .Include(c => c.Category)
                .Include(c => c.Publishing_House)
                .Select(c => new GetBookViewModel
                {
                    Id = c.Id,
                    Name = c.Name_en,
                    Image = c.Image,
                    Category_name = c.Category.Name_en
                }).AsQueryable();


            if (!string.IsNullOrEmpty(param.Key))
            {
                books = books.Where(c => c.Name.Contains(param.Key) 
                    || c.Category_name.Contains(param.Key));
            }

            return await PagedList<GetBookViewModel>
                .CreateAsync(books, param.PageNumber, param.PageSize);
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            return book;
        }

        public async Task<SaveBookViewModel> GetUpdatedBookById(int id)
        {
            var book = await GetBook(id);

            var model = new SaveBookViewModel
            {
                Id = book.Id,
                Name_ar = book.Name_ar,
                Name_en = book.Name_en,
                Name = _repoUser.GetUserNameById(GetBookAuthorId(id)),
                Publishing_house_id = book.Publishing_house_id,
                Pages_number = book.Pages_number,
                Date_of_publication = book.Date_of_publication,
                Short_description = book.Short_description,
                Category_id = book.Category_id,
                Categories = await _repoCategory.GetCategoriesDropDownList(),
                PublishingHouses = await _repoPhblishingHouse.GetPublishingHouses(),
                users = await _repoUser.GetUserDrobDownList()
            };

            return model;
        }

        public async Task<string> SaveBook(SaveBookViewModel model, string root)
        {
            if (model.Id == 0)
            {
                if (model.File == null)
                {
                    return "Please choose image";
                }

                string fileName;
                var saveImage = _repoCore.SaveSingleImage(root, model.File, out fileName);

                if (!saveImage)
                {
                    return "Upload failed";
                }

                if (await IsBookExistBefore(model.Name_en) || await IsBookExistBefore(model.Name_ar))
                {
                    return "Book already exist";
                }
                else
                {
                    var book = new Book
                    {
                        Name_ar = model.Name_ar,
                        Short_description = model.Short_description,
                        Name_en = model.Name_en,
                        Publishing_house_id = model.Publishing_house_id.Value,
                        Pages_number = model.Pages_number,
                        Image = fileName,
                        Date_of_publication = model.Date_of_publication,
                        Category_id = model.Category_id.Value
                    };

                    _repoCore.Add(book);
                    await _repoCore.SaveAll();

                    if (await _repoUser.IsUserExistInDb(model.Name) == false)
                    {
                        return "This User not exist";
                    }

                    BookAuthor author = new BookAuthor
                    {
                        Book_id = book.Id,
                        User_id = _repoUser.GetUserIdByName(model.Name)
                    };

                    _repoCore.Add(author);
                }
            }
            else
            {
                var _book = await GetBook(model.Id);

                if (model.File == null)
                {
                    model.Image = _book.Image;
                }
                else
                {
                    string fullPath = root + "/" + _book.Image;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string fileName;
                    var saveImage = _repoCore.SaveSingleImage(root, model.File, out fileName);

                    if (!saveImage)
                    {
                        return "Upload failed";
                    }

                    _book.Image = fileName;
                }

                _book.Name_ar = model.Name_ar;
                _book.Name_en = model.Name_en;
                _book.Publishing_house_id = model.Publishing_house_id.Value;
                _book.Pages_number = model.Pages_number;
                _book.Short_description = _book.Short_description;
                _book.Date_of_publication = model.Date_of_publication;
                _book.Category_id = model.Category_id.Value;
            }

            await _repoCore.SaveAll();

            return null;
        }

        public async Task<GetBookAndHisFeedBackViewModel> GetBookDetailsById(int id)
        {
            var book = await _context.Books
                .Include(c => c.Category)
                .Include(c => c.Publishing_House)
                .Include(c => c.Book_authors).ThenInclude(co => co.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            return new GetBookAndHisFeedBackViewModel { 
                Book = book,
                FeedBacks = _repoFeedBack.GetFeedBacks(id)
            };
        }

        public async Task<bool> IsBookExistBefore(string name)
        {
            var isBookInDb = await _context.Books.AnyAsync(b => b.Name_ar == name || b.Name_en == name);

            return isBookInDb;
        }

        public string GetBookAuthorId(int id)
        {
            var authorId = _context.BookAuthors.FirstOrDefault(b => b.Book_id == id).User_id;

            return authorId;
        }

        public int GetBookIdByName(string name)
        {
            var bookId = _context.Books.FirstOrDefault(b => b.Name_ar == name || b.Name_en == name).Id;

            return bookId;
        }

        public IQueryable<GetBookViewModel> GetBooks(UserParam param)
        {
            var books = _context.Books
                .Include(p => p.Publishing_House)
                .Include(c => c.Category)
                .Select(b => new GetBookViewModel
                { 
                    Id = b.Id,
                    Name = param.Lang == "en"? b.Name_en : b.Name_ar,
                    Category_name = param.Lang == "en"? b.Category.Name_en : b.Category.Name_ar,
                    Image = b.Image
                });

            return books;
        }

        public async Task<BookDetailsViewModel> bookDetails(int id, UserParam param)
        {
            var book = await _context.Books
                .Include(c => c.Category)
                .Include(c => c.Publishing_House)
                .Include(c => c.Book_authors).ThenInclude(co => co.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            return new BookDetailsViewModel
            {
                Id = book.Id,
                Name = param.Lang == "en" ? book.Name_en : book.Name_ar,
                Date_of_publication = book.Date_of_publication,
                Pages_number = book.Pages_number,
                Category_name = param.Lang == "en" ? book.Category.Name_en : book.Category.Name_ar,
                Image = book.Image,
                Book_Author = string.Join(",", book.Book_authors.Select(b => b.User.Name))
            };
        }

        public async Task<List<GetBooksDropDownListViewModel>> GetBooksDropDownList()
        {
            var books = await _context.Books
                .Select(b => new GetBooksDropDownListViewModel { 
                    Id = b.Id,
                    Name = b.Name_en
                }).ToListAsync();

            return books;
        }
    }
}

