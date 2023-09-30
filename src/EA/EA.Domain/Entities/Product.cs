using System.ComponentModel.DataAnnotations.Schema;
using EA.Domain.Primitives.Entities;
using EAnalytics.Common.Entities;
using EAnalytics.Common.Enums;

namespace EA.Domain.Entities
{
    [Table("ea_products")]
    public class Product : BaseEntity<long>
    {
        public virtual ICollection<SystemProduct> SystemProducts { get; set; }
        
        public virtual Category Category { get; set; }
    }
}