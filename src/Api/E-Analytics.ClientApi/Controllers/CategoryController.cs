using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EA.Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICommandHandler _handler;
        public CategoryController(ICommandHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }
        
    }
}