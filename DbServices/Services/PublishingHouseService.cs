using DbServices.Base;
using DbServices.Model;
using Entitis.Models;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.PublishingHouseVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class PublishingHouseService : BaseService , IPublishingHouse
    {
        private DataContext _context;
        private ICoreBase _repoCore;

        public PublishingHouseService(DataContext context,
            ICoreBase repoCore)
        {
            _context = context;
            _repoCore = repoCore;
        }

        public async Task<PagedList<GetPublishingHouseViewModel>> GetPublishingHousiesWithPadination(UserParam param)
        {
            var publishing_House = _context.publishingHouses
                .Select(c => new GetPublishingHouseViewModel
                {
                    Id = c.Id,
                    Name = c.Name_en
                }).AsQueryable();


            if (!string.IsNullOrEmpty(param.Key))
            {
                publishing_House = publishing_House
                    .Where(c => c.Name.Contains(param.Key));
            }

            return await PagedList<GetPublishingHouseViewModel>
                .CreateAsync(publishing_House, param.PageNumber, param.PageSize);
        }

        public async Task<PublishingHouse> GetPublishingHouse(int id)
        {
            var publishing_House = await _context.publishingHouses.FindAsync(id);

            return publishing_House;
        }

        public async Task<SavePublishingHouseViewModel> GetUpdatedPublishingHouseById(int id)
        {
            var publishing_House = await GetPublishingHouse(id);

            var model = new SavePublishingHouseViewModel
            {
                Id = publishing_House.Id,
                Name_ar = publishing_House.Name_ar,
                Name_en = publishing_House.Name_en,
                Address = publishing_House.Address,
                Phone = publishing_House.Phone,
                City = publishing_House.City,
                The_state = publishing_House.The_state,
                Email = publishing_House.Email
            };

            return model;
        }

        public async Task<string> SavePublishingHouse(SavePublishingHouseViewModel model)
        {
            if (model.Id == 0)
            {
                PublishingHouse publishingHouse = new PublishingHouse
                {
                    Name_ar = model.Name_ar,
                    Name_en = model.Name_en,
                    Address = model.Address,
                    Phone = model.Phone,
                    City = model.City,
                    The_state = model.The_state,
                    Email = model.Email
                };

                _repoCore.Add(publishingHouse);
            }
            else
            {
                var publishing_House = await GetPublishingHouse(model.Id);

                publishing_House.Id = model.Id;
                publishing_House.Name_ar = model.Name_ar;
                publishing_House.Name_en = model.Name_en;
                publishing_House.Address = model.Address;
                publishing_House.Phone = model.Phone;
                publishing_House.City = model.City;
                publishing_House.The_state = model.The_state;
                publishing_House.Email = model.Email;
                
            }

            await _repoCore.SaveAll();

            return null;
        }

        public async Task<List<GetPublishingHousesDrobDownListViewModel>> GetPublishingHouses()
        {
            var publishing_houses = await _context.publishingHouses
                .Select(p => new GetPublishingHousesDrobDownListViewModel { 
                    Id = p.Id,
                    Name = p.Name_ar
                }).ToListAsync();

            return publishing_houses;
        }

        public IQueryable<GetPublishingHouseViewModel> GetPublishingHouses(UserParam param)
        {
            var publishingHouses = _context.publishingHouses.
                Select(p => new GetPublishingHouseViewModel { 
                    Id = p.Id,
                    Name = param.Lang == "en"? p.Name_en : p.Name_ar,
                    Phone = p.Phone,
                    Address = p.Address,
                    City = p.City,
                    The_state = p.The_state,
                    Email = p.Email
                });

            return publishingHouses;
        }

        public async Task<PublishingHouseDetailsViewModel> GetPublishingHouseDetails(int id)
        {
            var publishing_House = await GetPublishingHouse(id);

            return new PublishingHouseDetailsViewModel
            {
                Id = publishing_House.Id,
                Name = publishing_House.Name_en,
                Address = publishing_House.Address,
                Phone = publishing_House.Phone,
                Email = publishing_House.Email,
                City = publishing_House.City,
                The_state = publishing_House.The_state
            };
        }
    }
}
