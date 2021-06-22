using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.CollegeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class CollegeService : BaseService , ICollege
    {
        private DataContext _context;
        private ICoreBase _repoCore;

        public CollegeService(DataContext context,
            ICoreBase repoCore)
        {
            _context = context;
            _repoCore = repoCore;
        }

        public async Task<PagedList<GetCollegeViewModel>> GetCollegiesWithPadination(UserParam param)
        {
            var collegies = _context.Colleges
                .Select(c => new GetCollegeViewModel
                {
                    Id = c.Id,
                    Name = c.Name_en
                }).AsQueryable();


            if (!string.IsNullOrEmpty(param.Key))
            {
                collegies = collegies.Where(c => c.Name.Contains(param.Key));
            }

            return await PagedList<GetCollegeViewModel>
                .CreateAsync(collegies, param.PageNumber, param.PageSize);
        }

        public async Task<College> GetCollege(int id)
        {
            var college = await _context.Colleges.FindAsync(id);

            return college;
        }

        public async Task<SaveCollegeViewModel> GetUpdatedCollegeById(int id)
        {
            var category = await GetCollege(id);

            var model = new SaveCollegeViewModel
            {
                Id = category.Id,
                Name_ar = category.Name_ar,
                Name_en = category.Name_en
            };

            return model;
        }

        public async Task<string> SaveCollege(SaveCollegeViewModel model)
        {
            if (model.Id == 0)
            {
                if (await IsCollegeNameExist(model.Name_ar) || await IsCollegeNameExist(model.Name_en))
                {
                    return "name is already exist before";
                }
                else
                {
                    College college = new College
                    {
                        Name_ar = model.Name_ar,
                        Name_en = model.Name_en
                    };

                    _repoCore.Add(college);
                }
            }
            else
            {
                var _college = await GetCollege(model.Id);

                _college.Name_ar = model.Name_ar;
                _college.Name_en = model.Name_en;
            }

            await _repoCore.SaveAll();

            return null;
        }

        public async Task<IEnumerable<College>> GetColleges()
        {
            var colleges = await _context.Colleges.ToListAsync();

            return colleges;
        }

        public async Task<bool> IsCollegeNameExist(string name)
        {
            var isCollegeInDb = await _context.Colleges
                .AnyAsync(c => c.Name_ar == name || c.Name_en == name);

            return isCollegeInDb;
        }

        public IQueryable<GetCollegeViewModel> GetColleges(UserParam param)
        {
            var college = _context.Colleges
                .Select(c => new GetCollegeViewModel { 
                    Id = c.Id,
                    Name = param.Lang == "en"? c.Name_en : c.Name_ar
                });

            return college;
        }

        public async Task<List<GetCollegeDrobDownListViewModel>> GetCollegesDrobDownList()
        {
            var colleges = await _context.Colleges
                .Select(c => new GetCollegeDrobDownListViewModel { 
                    Id = c.Id,
                    Name = c.Name_en
                }).ToListAsync();

            return colleges;
        }
    }
}
