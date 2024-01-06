using EAnalytics.Common.Dtos;
using OL.Domain.Primitives.Entities;

namespace OL.Domain.Mappers;

public static class TranslationMapper
{
    public static OLTranslation ToEntity(TranslationDto translation) => new OLTranslation()
    {
        LanguageCode = translation.LanguageCode.Code,
        Title = translation.Title,
        Description = translation.Description,
        Id = translation.Id
    };
}
