using System.ComponentModel.DataAnnotations.Schema;
using EAnalytics.Common.Dtos;
using EAnalytics.Common.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Domain.Entities;

[Table("ol_product")]
public class OLProduct : BaseEntity<Guid>
{
    public long SystemId { get; set; }
    public int InstalmentMaxMouth { get; set; }
    public decimal InstalmentMonthlyRepayment { get; set; }
    public long[] SystemCategoryId { get; set; }
    public virtual IList<OLTranslation> Translations { get; set; }
    public virtual IList<OLProductPriceHistory> Price { get; set; }
}

[Table("ol_product_price_history")]
public class OLProductPriceHistory : BaseEntity<Guid>
{
    public decimal Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
