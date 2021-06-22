using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.CollegeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICollege : IService
    {
        IQueryable<GetCollegeViewModel> GetColleges(UserParam param);
        Task<bool> IsCollegeNameExist(string name);
        Task<IEnumerable<College>> GetColleges();
        Task<SaveCollegeViewModel> GetUpdatedCollegeById(int id);
        Task<College> GetCollege(int id);
        Task<string> SaveCollege(SaveCollegeViewModel model);
        Task<List<GetCollegeDrobDownListViewModel>> GetCollegesDrobDownList();
        Task<PagedList<GetCollegeViewModel>> GetCollegiesWithPadination(UserParam param);
    }
}
