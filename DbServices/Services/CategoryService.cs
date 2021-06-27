using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.CategoryVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class CategoryService : BaseService, ICategory
    {
        private DataContext _context;
        private ICoreBase _repoCore;

        public CategoryService(DataContext context,
            ICoreBase repoCore)
        {
            _context = context;
            _repoCore = repoCore;
        }

        public IQueryable<GetCategoryViewModel> GetCategories(UserParam param)
        {
            var categories = _context.Categories
                .Select(c => new GetCategoryViewModel
                {
                    Id = c.Id,
                    Name = param.Lang == "en" ? c.Name_en : c.Name_ar
                });

            return categories;
        }

        public async Task<List<GetCategoriesDrobDownListViewModel>> GetCategoriesDropDownList()
        {
            var categories = await _context.Categories
                .Select(c => new GetCategoriesDrobDownListViewModel { 
                    Id = c.Id,
                    Name = c.Name_ar
                }).ToListAsync();

            return categories;
        }

        public async Task<PagedList<GetCategoryViewModel>> GetCategoriesWithPadination(UserParam param)
        {
            var categories = _context.Categories
                .Select(c => new GetCategoryViewModel { 
                    Id = c.Id,
                    Name = c.Name_en
                }).AsQueryable();


            if (!string.IsNullOrEmpty(param.Key))
            {
                categories = categories.Where(c => c.Name.Contains(param.Key));
            }

            return await PagedList<GetCategoryViewModel>
                .CreateAsync(categories, param.PageNumber, param.PageSize);
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            return category;
        }

        public async Task<SaveCategoryViewModel> GetUpdatedCategoryById(int id)
        {
            var category = await GetCategory(id);

            var model = new SaveCategoryViewModel
            {
                Id = category.Id,
                Name_ar = category.Name_ar,
                Name_en = category.Name_en
            };

            return model;
        }

        public async Task<string> SaveCategory(SaveCategoryViewModel model)
        {
            if (model.Id == 0)
            {
                var category = new Category
                {
                    Name_ar = model.Name_ar,
                    Name_en = model.Name_en
                };

                _repoCore.Add(category);
            }
            else
            {
                var category = await GetCategory(model.Id);

                category.Name_ar = model.Name_ar;
                category.Name_en = model.Name_en;
            }

            await _repoCore.SaveAll();

            return null;
        }

        
    }
}
