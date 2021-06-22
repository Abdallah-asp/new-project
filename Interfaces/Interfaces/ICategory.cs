using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICategory : IService
    {
        IQueryable<GetCategoryViewModel> GetCategories(UserParam param);
        Task<SaveCategoryViewModel> GetUpdatedCategoryById(int id);
        Task<Category> GetCategory(int id);
        Task<string> SaveCategory(SaveCategoryViewModel model);
        Task<PagedList<GetCategoryViewModel>> GetCategoriesWithPadination(UserParam param);
        Task<List<GetCategoriesDrobDownListViewModel>> GetCategoriesDropDownList();
    }
}
