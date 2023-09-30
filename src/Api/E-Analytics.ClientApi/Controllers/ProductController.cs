using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EA.Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandHandler _handler;
        public ProductController(ICommandHandler handler)
        {
            _handler = handler;
        }

        
        
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromRoute]Guid id, [FromBody] AddProductCommand command)
        {
            command.Id = id;
            
            await _handler.HandleAsync(command);
            return Ok();
        }
        
    }
}