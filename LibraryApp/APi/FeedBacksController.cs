using Interfaces.Helpers;
using Interfaces.Interfaces;
using Interfaces.ViewModel.FeedBackVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryApp.APi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {
        private IFeedBack _repo;
        private ICoreBase _repoCore;

        public FeedBacksController(IFeedBack repo,
            ICoreBase repoCore)
        {
            _repo = repo;
            _repoCore = repoCore;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveFeedBack([FromBody]SaveFeedBackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { messageError = 2 });
            }

            var res = await _repo.SaveFeedBack(model, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _repoCore.SaveAll();

            if (res == null)
            {
                return Ok(new { messageSuccess = 1 });
            }

            return BadRequest(new { messageError = 2 });
        }
    }
}
