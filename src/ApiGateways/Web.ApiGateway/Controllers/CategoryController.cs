using Microsoft.AspNetCore.Mvc;
using OL.Infrastructure.Commands.Categories;
using Web.ApiGateway.Controllers.Base;
using Web.ApiGateway.Services;

namespace Web.ApiGateway.Controllers
{
    public class CategoryController : BaseCommandController
    {
        private readonly OLCategoryService _categoryService;
        public CategoryController(OLCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleCreate([FromBody] AddOLCategoryCommand command)
        {
            await _categoryService.AddOLCategoryCommand(command);
            return Ok();
        }
    }
}