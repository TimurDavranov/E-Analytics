using Microsoft.AspNetCore.Mvc;

namespace Web.ApiGateway.Controllers.Base
{
    [ApiController]
    [Route("command/[controller]/[action]")]
    public class BaseCommandController : ControllerBase
    {
    }
}