using EAnalytics.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Commands.Product;

namespace Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProductController(ICommandDispatcher commandDispatcher) : ControllerBase
{
    public async Task<IActionResult> Create([FromBody] AddOlProductCommand command)
    {
        command.Id = Guid.NewGuid();
        await commandDispatcher.SendAsync(command);
        return Ok();
    }

    public async Task<IActionResult> Update([FromBody] UpdateOlProductCommand command)
    {
        if (command.Id == Guid.Empty)
            throw new ArgumentNullException("Category Id is null or empty!");
        
        await commandDispatcher.SendAsync(command);
        return Ok();
    }
}
