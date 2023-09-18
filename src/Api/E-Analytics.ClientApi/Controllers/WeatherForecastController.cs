using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ICommandHandler _handler;
        public WeatherForecastController(ICommandHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> HandleCreate([FromBody] AddCategoryCommand command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> HandleEdit([FromBody] EditCategoryCommand command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }
    }
}