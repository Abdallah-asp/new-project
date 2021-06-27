using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.FeedBackVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class FeedBackService : BaseService , IFeedBack
    {
        private DataContext _context;
        private ICoreBase _repoCore;

        public FeedBackService(DataContext context,
            ICoreBase repoCore)
        {
            _context = context;
            _repoCore = repoCore;
        }

        public async Task<FeedBack> GetFeedBack(int id)
        {
            var feedBack = await _context.FeedBacks.FindAsync(id);

            return feedBack;
        }

        public IQueryable<FeedBack> GetFeedBacks(int id)
        {
            var feedBacks = _context.FeedBacks
                .Include(u => u.User)
                .Where(f => f.Book_id == id);

            return feedBacks;
        }

        public async Task<string> SaveFeedBack(SaveFeedBackViewModel model, string userId)
        {
            if (model.Id == 0)
            {
                FeedBack feedBack = new FeedBack
                {
                    Feed_back = model.Feed_back,
                    Date = DateTime.UtcNow,
                    Rate = model.Rate,
                    Book_id = model.Book_id,
                    User_id = userId
                };

                _repoCore.Add(feedBack);
            }
            else
            {
                var feedBack = await GetFeedBack(model.Id);

                feedBack.Feed_back = model.Feed_back;

                await _repoCore.SaveAll();
            }

            return null;
        }
    }
}
