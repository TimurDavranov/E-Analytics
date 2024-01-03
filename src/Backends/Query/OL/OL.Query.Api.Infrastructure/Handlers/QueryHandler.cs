using EAnalytics.Common;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Dtos;
using Microsoft.EntityFrameworkCore;
using OL.Domain.Entities;
using OL.Infrastructure;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Query.Api.Infrastructure.Handlers
{
    public interface IQueryHandler
    {
        Task<CategoryResponse> HandleAsync(CategoryByIdRequest request);
        Task<CategoryResponse> HandleAsync(CategoryBySystemIdRequest request);
        Task<CategoryResponse> HandleAsync(CategoryByNameRequest request);
    }

    public sealed class QueryHandler : IQueryHandler
    {
        private readonly IRepository<OLCategory, OLDbContext> _repository;

        public QueryHandler(IRepository<OLCategory, OLDbContext> repository)
        {
            _repository = repository;
        }

        public async Task<CategoryResponse> HandleAsync(CategoryByIdRequest request)
        {
            var model = await _repository.GetAsync(s => s.Id == request.Id);

            if (model is null)
                return null;

            return new CategoryResponse(model.Id, model.SystemId, model.ParrentId,
                model.Translations.Select(s => new TranslationDto
                {
                    Description = s.Description,
                    LanguageCode = new LanguageCode(s.LanguageCode)
                }).ToList().AsReadOnly()
            );
        }

        public async Task<CategoryResponse> HandleAsync(CategoryBySystemIdRequest request)
        {
            var model = await _repository.GetAsync(s => s.SystemId == request.SystemId, i => i.Include(s=>s.Translations));

            if (model is null)
                return null;

            return new CategoryResponse(model.Id, model.SystemId, model.ParrentId,
                model.Translations.Select(s => new TranslationDto
                {
                    Description = s.Description,
                    LanguageCode = new LanguageCode(s.LanguageCode)
                }).ToList().AsReadOnly()
            );
        }

        public async Task<CategoryResponse> HandleAsync(CategoryByNameRequest request)
        {
            var model = await _repository.GetAsync(expression: s => s.Equals(request.Translations));

            if (model is null)
                return null;

            return new CategoryResponse(model.Id, model.SystemId, model.ParrentId,
                model.Translations.Select(s => new TranslationDto
                {
                    Description = s.Description,
                    LanguageCode = new LanguageCode(s.LanguageCode)
                }).ToList().AsReadOnly()
            );
        }
    }
}
