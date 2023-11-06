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
    public async Task<IActionResult> HandleCreate(AddOLCategoryCommand command)
    {
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> HandleEnable(EnableOLCategoryCommand command)
    {
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }
}