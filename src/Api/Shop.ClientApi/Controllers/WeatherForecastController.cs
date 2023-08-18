using Infrastructure.Repositories.Olcha.uz;
using Microsoft.AspNetCore.Mvc;

namespace Shop.ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly OlchaParserRepository olchaParserRepository;

        public WeatherForecastController()
        {
            olchaParserRepository = new();
        }

        [HttpGet]
        public IActionResult Parser() 
        {
            olchaParserRepository.ParseCategories();
            return Ok();
        }
    }
}