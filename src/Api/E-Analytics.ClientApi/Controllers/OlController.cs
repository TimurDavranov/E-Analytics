using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Commands.Categories;

namespace Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OlController : ControllerBase
{
    public OlController()
    {
    }

    [HttpPost]
    public async Task<IActionResult> HandleCreate(AddCategoryCommand command)
    {
        return Ok();
    }
}