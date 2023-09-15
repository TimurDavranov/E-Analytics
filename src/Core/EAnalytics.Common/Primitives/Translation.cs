using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EAnalytics.Common.Entities;

namespace EAnalytics.Common.Primitives
{
    public class Translation : BaseEntity<Guid>
    {
        [MaxLength(10), MinLength(2), NotNull]
        public string LanguageCode { get; set; }
        [MinLength(3), MaxLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
