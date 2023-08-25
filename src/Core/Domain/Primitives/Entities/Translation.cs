using System;
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
    public class Translation : Base
    {
        [MaxLength(10), MinLength(10), NotNull]
        public required string LanguageCode { get; set; }
        [MinLength(3), MaxLength(255)]
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
