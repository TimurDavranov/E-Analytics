using System.ComponentModel.DataAnnotations.Schema;
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
}
