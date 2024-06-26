﻿using System.ComponentModel.DataAnnotations.Schema;
using EAnalytics.Common.Entities;
using EAnalytics.Common.Enums;

namespace EA.Domain.Entities;

[Table("ea_system_products")]
public class SystemProduct : BaseEntity<Guid>
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public SystemName SystemName { get; set; }

    public Guid ProductId { get; set; }

    public string Url { get; set; }

    public virtual Product Product { get; set; }

}