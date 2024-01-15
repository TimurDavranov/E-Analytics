using EAnalytics.Common.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    public class UserController : BaseApiController
    {

        [HttpGet]
        public IActionResult Login()
        {
            return Ok("");
        }
    }
}
