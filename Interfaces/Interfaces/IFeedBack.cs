using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.FeedBackVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IFeedBack : IService
    {
        Task<FeedBack> GetFeedBack(int id);
        Task<string> SaveFeedBack(SaveFeedBackViewModel model, string userId);
        IQueryable<FeedBack> GetFeedBacks(int id);
    }
}
