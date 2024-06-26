using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EAnalytics.Common.Commands;
using Microsoft.AspNetCore.Mvc;

namespace EA.Command.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public CategoryController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
        {
            
            var categoryCommand = new AddCategoryCommand()
            {
                Id = Guid.NewGuid(),
                Translations = command.Translations,
                Parent = command.Parent 
                
            };
            await _commandDispatcher.SendAsync(categoryCommand);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

    }
}