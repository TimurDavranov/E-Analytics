using EA.Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace EA.Domain.Entities.Olcha;

[Table("OL_categories")]
public class OLCategory : BaseEntity<Guid>
{
    public OLCategory(long categoryId, bool isActive, long? parrentId, List<OLCategory> childs, List<Transaction> transactions)
    {
        CategoryId = categoryId;
        IsActive = isActive;
        ParrentId = parrentId;
        Childs = childs;
        Transactions = transactions;
    }

    public long CategoryId { get; set; }
    public bool IsActive { get; set; }
    public long? ParrentId { get; set; }
    public virtual List<OLCategory> Childs { get; set; }
    public virtual List<Transaction> Transactions { get; set; }
}
