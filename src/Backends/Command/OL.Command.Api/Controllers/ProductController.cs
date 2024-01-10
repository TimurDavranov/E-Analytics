using EAnalytics.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Commands.Product;

namespace Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProductController(ICommandDispatcher commandDispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddOlProductCommand command)
    {
        command.Id = Guid.NewGuid();
        await commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateOlProductCommand command)
    {
        if (command.Id == Guid.Empty)
            throw new ArgumentNullException("Category Id is null or empty!");

        await commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRange([FromBody] List<AddOlProductCommand> commands)
    {
        foreach (var command in commands)
        {
            command.Id = Guid.NewGuid();
            await commandDispatcher.SendAsync(command);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRange([FromBody] List<UpdateOlProductCommand> commands)
    {
        if (commands.Any(s => s.Id == Guid.Empty))
            throw new ArgumentNullException("Category Id is null or empty!");

        foreach (var command in commands)
            await commandDispatcher.SendAsync(command);
        return Ok();
    }
}