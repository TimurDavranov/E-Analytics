using System.ComponentModel.DataAnnotations.Schema;
using EAnalytics.Common.Dtos;
using EAnalytics.Common.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Domain.Entities;

[Table("ol_categories")]
public class OLCategory : BaseEntity<Guid>
{
    public bool Enabled { get; set; }
    public long SystemId { get; set; }
    public long? ParrentId { get; set; }
    public virtual IList<OLTranslation> Translations { get; set; }

    public override bool Equals(object translations)
    {
        var model = translations as List<TranslationDto>;

        if (model is null)
            return false;

        foreach (var item in model)
        {
            if (this.Translations.Any(s => s.LanguageCode == item.LanguageCode.Code && s.Title.Equals(item.Title, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
        }
        return false;
    }
}
