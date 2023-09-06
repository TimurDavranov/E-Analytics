﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives.Entities
{
    [Table("translations")]
    public class Translation : BaseEntity<Guid>
    {
        [MaxLength(10), MinLength(10), NotNull]
        public string LanguageCode { get; set; }
        [MinLength(3), MaxLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
