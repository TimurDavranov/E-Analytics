using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Requests.Category;

public class CategoryByNameRequest : BaseQuery
{
    public List<TranslationDto> Translations { get; set; }
}