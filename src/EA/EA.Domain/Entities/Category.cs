using System.ComponentModel.DataAnnotations.Schema;
using EA.Domain.Primitives.Entities;
using EAnalytics.Common.Entities;

namespace EA.Domain.Entities
{
    [Table("ea_categories")]
    public class Category : BaseEntity<Guid>
    {
        public virtual List<CategoryRelation>? Relations { get; set; }
        public virtual List<CategoryRelation>? Parents { get; set; }
        public virtual List<EACategoryTranslation> Translations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}