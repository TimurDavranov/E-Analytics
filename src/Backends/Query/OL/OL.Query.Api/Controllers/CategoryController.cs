using EAnalytics.Common.Primitives;
using EAnalytics.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Query.Api.Controllers
{
    public class CategoryController(IQueryDispatcher dispatcher) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> GetById([FromBody] CategoryByIdRequest request)
        {
            return Ok(await dispatcher.SendAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> GetBySystemId([FromBody] CategoryBySystemIdRequest request)
        {
            return Ok(await dispatcher.SendAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIds()
        {
            return Ok(await dispatcher.SendAsync(new GetAllRequest()));
        }

    }
}
