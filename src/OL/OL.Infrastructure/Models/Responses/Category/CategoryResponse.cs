using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Responses.Category;

public class CategoryResponse : BaseQueryResponse
{
    public CategoryResponse(Guid Id, long SystemId, long? ParentId, IReadOnlyList<TranslationDto> Translations)
    {
        this.Id = Id;
        this.SystemId = SystemId;
        this.ParentId = ParentId;
        this.Translations = Translations;
    }

    public Guid Id { get; init; }
    public long SystemId { get; init; }
    public long? ParentId { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
