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
    public class CollegiesController : ControllerBase
    {
        private ICollege _repo;
        private ICoreBase _repoCore;

        public CollegiesController(ICollege repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllCollegies([FromQuery] UserParam param)
        {
            var books = _repo.GetColleges(param);

            return Ok(books);
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

                var collegies = await _repo.GetCollegiesWithPadination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = collegies.TotalCount,
                    recordsTotal = collegies.TotalCount,
                    data = collegies
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

