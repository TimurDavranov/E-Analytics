using Domain.Abstraction;
using Domain.Abstraction.Repositories;
using Domain.Abstraction.Repositories.Olcha;
using Domain.Entities.Olcha;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories.Olcha;

public sealed class OlchaCategoryReadRepository : ReadRepository<OlchaCategory>, IOlchaCategoryReadRepository
{
    private readonly IApplicationDbContext _context;
    public OlchaCategoryReadRepository(IApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override IQueryable<OlchaCategory> GetQueryable() =>
        _context.OlchaCategories.AsQueryable();

    public override Task<OlchaCategory?> GetByIdAsync(long id, Func<IQueryable<OlchaCategory>, IIncludableQueryable<OlchaCategory, object>>? include = null) =>
        include is not null ? include(_context.OlchaCategories.Where(s => s.Id == id)).FirstOrDefaultAsync() : _context.OlchaCategories.FirstOrDefaultAsync(s => s.Id == id);

    public Task<OlchaCategory?> GetByOlchaIdAsync(long id, Func<IQueryable<OlchaCategory>, IIncludableQueryable<OlchaCategory, object>>? include = null) =>
        include is not null ? include(_context.OlchaCategories.Where(s => s.CategoryId == id)).FirstOrDefaultAsync() : _context.OlchaCategories.FirstOrDefaultAsync(s => s.CategoryId == id);
}
