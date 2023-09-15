using System.ComponentModel.DataAnnotations.Schema;
using EAnalytics.Common.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Domain.Entities;

[Table("ol_categories")]
public class OLCategory : BaseEntity<Guid>
{
    public bool IsActive { get; set; }
    public virtual List<OLCategory> Parrents { get; set; }
    public virtual List<OLCategory> Childs { get; set; }
    public virtual List<OLTranslation> Translations { get; set; }
}
