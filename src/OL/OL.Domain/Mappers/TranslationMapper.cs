using EAnalytics.Common;
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

    public static TranslationDto ToModel(OLTranslation translation) => new TranslationDto()
    {
        LanguageCode = new LanguageCode(translation.LanguageCode),
        Title = translation.Title,
        Description = translation.Description
    };
}
