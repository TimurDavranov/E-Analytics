using EAnalytics.Common.Primitives;
using EAnalytics.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Query.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
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

        [HttpPost]
        public async Task<IActionResult> GetByName([FromBody] CategoryByNameRequest request)
        {
            return Ok(await dispatcher.SendAsync(request));
        }

    }
}
