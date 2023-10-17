using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EA.Infrastructure.Handlers;
using EAnalytics.Common.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public ProductController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        
        
        [HttpPost]
        public async Task<IActionResult> AddProduct(Guid id, [FromBody] AddProductCommand command)
        {
			if (id == Guid.Empty)
			{
				throw new Exception("Guid parameter is wrong!");
			}

			command.Id = id;
            
            
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
        
    }
}