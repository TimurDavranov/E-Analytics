using System.ComponentModel.DataAnnotations.Schema;
using EA.Domain.Primitives.Entities;
using EAnalytics.Common.Entities;

namespace EA.Domain.Entities
{
    [Table("ea_categories_relations")]
    public class CategoryRelation : BaseEntity<int>
    {
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Category Parent { get; set; }

    }
}