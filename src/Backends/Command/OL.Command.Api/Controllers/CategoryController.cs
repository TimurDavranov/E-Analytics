using EAnalytics.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Commands.Categories;

namespace Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    public CategoryController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddOlCategoryCommand command)
    {
        command.Id = Guid.NewGuid();
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateOlCategoryCommand command)
    {
        if (command.Id == Guid.Empty)
            throw new ArgumentNullException("Category Id is null or empty!");

        await _commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> HandleEnable([FromBody] EnableOLCategoryCommand command)
    {
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }
}