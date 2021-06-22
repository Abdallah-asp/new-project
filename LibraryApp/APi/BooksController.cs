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
    public class BooksController : ControllerBase
    {
        private IBook _repo;
        private ICoreBase _repoCore;

        public BooksController(IBook repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        [Authorize]
        [Route("getBooks")]
        [HttpGet]
        public IActionResult GetAllBooks([FromQuery]UserParam param)
        {
            var books = _repo.GetBooks(param);
            return Ok(books);
        }

        [Authorize]
        [Route("book/{id}/details")]
        [HttpGet]
        public async Task<IActionResult> GetDetails([FromRoute]int id, [FromQuery] UserParam param)
        {
            var details = await _repo.bookDetails(id, param);

            return Ok(details);
        }

        [Authorize(Policy = "AdminRequire")]
        [HttpPost]
        public async Task<IActionResult> GetBooksWithPagiation()
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

                var books = await _repo.GetBooksWithPadination(new UserParam
                {
                    PageNumber = PageNumber,
                    PageSize = pageSize,
                    Key = searchValue
                });

                // Send to View 
                var jsonData = new
                {
                    recordsFiltered = books.TotalCount,
                    recordsTotal = books.TotalCount,
                    data = books
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