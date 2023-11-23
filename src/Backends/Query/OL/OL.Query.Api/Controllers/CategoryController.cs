using EAnalytics.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Query.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IQueryDispatcher _dispatcher;
        public CategoryController(IQueryDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> GetById([FromBody] CategoryByIdRequest request)
        {
            return Ok(await _dispatcher.SendAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> GetBySystemId([FromBody] CategoryBySystemIdRequest request)
        {
            return Ok(await _dispatcher.SendAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse?>> GetByName([FromBody] CategoryByNameRequest request)
        {
            return Ok(await _dispatcher.SendAsync(request));
        }

    }
}
