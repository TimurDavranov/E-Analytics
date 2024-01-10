using EAnalytics.Common.Primitives;
using EAnalytics.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Models.Requests.Product;

namespace OL.Query.Api.Controllers;

public class ProductController(IQueryDispatcher dispatcher) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> GetBySystemId([FromBody] ProductBySystemIdRequest request)
    {
        return Ok(await dispatcher.SendAsync(request));
    }
    
    [HttpPost]
    public async Task<IActionResult> GetBySystemIds([FromBody] ProductBySystemIdsRequest request)
    {
        return Ok(await dispatcher.SendAsync(request));
    }
}
