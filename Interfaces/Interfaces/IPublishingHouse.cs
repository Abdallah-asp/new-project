using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.PublishingHouseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IPublishingHouse : IService
    {
        IQueryable<GetPublishingHouseViewModel> getPublishingHouses(UserParam param);
        Task<SavePublishingHouseViewModel> GetUpdatedPublishingHouseById(int id);
        Task<PublishingHouse> GetPublishingHouse(int id);
        Task<string> SavePublishingHouse(SavePublishingHouseViewModel model);
        Task<PagedList<GetPublishingHouseViewModel>> GetPublishingHousiesWithPadination(UserParam param);
        Task<List<GetPublishingHousesDrobDownListViewModel>> GetPublishingHouses();
    }
}
