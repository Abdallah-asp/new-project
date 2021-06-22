using Interfaces.Helpers;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.APi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategory _repo;
        private ICoreBase _repoCore;

        public CategoriesController(ICategory repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllCategories([FromQuery] UserParam param)
        {
            var categories = _repo.GetCategories(param);

            return Ok(categories);
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

                var categories = await _repo.GetCategoriesWithPadination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = categories.TotalCount,
                    recordsTotal = categories.TotalCount,
                    data = categories
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
