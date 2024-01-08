using EAnalytics.Common;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;
using Microsoft.EntityFrameworkCore;
using OL.Domain.Entities;
using OL.Domain.Mappers;
using OL.Domain.Primitives.Entities;
using OL.Infrastructure;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Requests.Product;
using OL.Infrastructure.Models.Responses.Category;
using OL.Infrastructure.Models.Responses.Product;

namespace OL.Query.Api.Infrastructure.Handlers
{
    public interface IQueryHandler
    {
        Task<CategoryResponse> HandleAsync(CategoryByIdRequest request);
        Task<CategoryResponse> HandleAsync(CategoryBySystemIdRequest request);
        Task<GetAllResponse<CategoryIdsResponse>> HandleAsync(GetAllRequest request);
        
        Task<ProductResponse> HandleAsync(ProductBySystemIdRequest request);
    }

    public sealed class QueryHandler(
        IRepository<OLCategory, OLDbContext> repository,
        IRepository<OLProduct, OLDbContext> productRepository)
        : IQueryHandler
    {
        public async Task<CategoryResponse> HandleAsync(CategoryByIdRequest request)
        {
            var model = await repository.GetAsync(s => s.Id == request.Id);

            if (model is null)
                return null;

            return new CategoryResponse(model.Id, model.SystemId, model.ParrentId,
                model.Translations.Select(s => new TranslationDto
                {
                    Description = s.Description,
                    LanguageCode = new LanguageCode(s.LanguageCode),
                    Title = s.Title
                }).ToList().AsReadOnly()
            );
        }

        public async Task<CategoryResponse> HandleAsync(CategoryBySystemIdRequest request)
        {
            var model = await repository.GetAsync(s => s.SystemId == request.SystemId,
                i => i.Include(s => s.Translations));

            if (model is null)
                return null;

            return new CategoryResponse(model.Id, model.SystemId, model.ParrentId,
                model.Translations.Select(s => new TranslationDto
                {
                    Description = s.Description,
                    LanguageCode = new LanguageCode(s.LanguageCode),
                    Title = s.Title
                }).ToList().AsReadOnly()
            );
        }

        public async Task<GetAllResponse<CategoryIdsResponse>> HandleAsync(GetAllRequest request)
        {
            return new GetAllResponse<CategoryIdsResponse>()
            {
                Data = await repository.GetAllAsync(expression: null, include: null, s =>
                    new CategoryIdsResponse
                    {
                        Id = s.Id,
                        SystemId = s.SystemId
                    })
            };
        }

        public async Task<ProductResponse> HandleAsync(ProductBySystemIdRequest request)
        {
            var model = await productRepository.GetAsync(s => s.SystemId == request.SystemId);
            if (model is null)
                return null;

            return new ProductResponse()
            {
                SystemId = model.SystemId,
                Id = model.Id,
                Price = model.Price,
                InstalmentMaxMouth = model.InstalmentMaxMouth,
                InstalmentMonthlyRepayment = model.InstalmentMonthlyRepayment,
                Translations = model.Translations.Select(TranslationMapper.ToModel).ToArray()
            };
        }
    }
}