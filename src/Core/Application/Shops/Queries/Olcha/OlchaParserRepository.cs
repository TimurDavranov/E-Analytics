using Application.Abstractions;
using Domain.Abstraction.Repositories.Olcha;
using Microsoft.EntityFrameworkCore;

namespace Application.Shops.Queries.Olcha
{
    public class OlchaParserRepository : IOlchaParserRepository
    {
        private readonly IOlchaConnectionService _connectionService;
        private readonly IOlchaCategoryReadRepository _repositroy;

        public OlchaParserRepository(IOlchaConnectionService connectionService, IOlchaCategoryReadRepository repositroy)
        {
            _connectionService = connectionService;
            _repositroy = repositroy;
        }

        public async Task ParseCategories()
        {
            var response = await _connectionService.GetCategories();
            var categories = await _repositroy.GetQueryable().Where(s => response.Data.Categories.Any(a => a.Id == s.CategoryId)).ToListAsync();
            response.Data.Categories.ToList().ForEach(res =>
            {
                if (categories.Any(cat => cat.CategoryId == res.Id))
                {
                    // to update
                }
                else
                {
                    // to insert
                }
            });

            categories.ToList().ForEach(cat =>
            {
                if (response.Data.Categories.Any(s => s.Id != cat.Id))
                {
                    // to remove
                }
            });

            if (_repositroy.HasChanges())
            {
                // save changes
            }
        }
    }
}
