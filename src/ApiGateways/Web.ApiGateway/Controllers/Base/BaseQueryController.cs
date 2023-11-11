using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.ApiGateway.Controllers.Base
{
    [ApiController]
    [Route("query/[controller]/[action]")]
    public class BaseQueryController : ControllerBase
    {

    }
}