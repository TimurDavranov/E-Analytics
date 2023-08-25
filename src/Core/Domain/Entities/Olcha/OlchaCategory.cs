using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Entities.Olcha;

[Table("olcha_categories")]
public class OlchaCategory : Base
{
    public OlchaCategory(long categoryId, bool isActive, long? parrentId, List<OlchaCategory> childs, List<Transaction> transactions)
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
    public virtual List<OlchaCategory> Childs { get; set; }
    public virtual List<Transaction> Transactions { get; set; }
}
