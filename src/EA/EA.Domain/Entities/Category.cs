using System.ComponentModel.DataAnnotations.Schema;
using EA.Domain.Primitives.Entities;
using EAnalytics.Common.Entities;

namespace EA.Domain.Entities
{
    [Table("ea_categories")]
    public class Category : BaseEntity<long>
    {
        public virtual List<Category> Parent { get; set; }
        public virtual List<Category> Childs { get; set; }
        public virtual List<EATranslation> Translations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}