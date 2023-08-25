using Application.Abstractions;
using Infrastructure.Repositories.Olcha;
using Microsoft.AspNetCore.Mvc;

namespace Shop.ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IOlchaParserRepository _olchaParserRepository;

        public WeatherForecastController(IOlchaParserRepository olchaParserRepository)
        {
            _olchaParserRepository = olchaParserRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Parser() 
        {
            var result = await _olchaParserRepository.ParseCategories();
            return Ok(result);
        }
    }
}