using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.Helpers;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.APi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishingHousiesController : ControllerBase
    {
        private IPublishingHouse _repo;
        private ICoreBase _repoCore;

        public PublishingHousiesController(IPublishingHouse repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllPublishingHouses([FromQuery] UserParam param)
        {
            var publishingHouses = _repo.getPublishingHouses(param);

            return Ok(publishingHouses);
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost]
        public async Task<IActionResult> GetCategoriesWithPagiation()
        {
            try
            {
                // Datatable Pagination Server Side Properties
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int PageNumber = (int.Parse(start) / pageSize);
                // Datatable Properties End

                var Publishing_House = await _repo.GetPublishingHousiesWithPadination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = Publishing_House.TotalCount,
                    recordsTotal = Publishing_House.TotalCount,
                    data = Publishing_House
                };
                // Send to View End

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
