using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Domain.Primitives.DTOs;

namespace EA.Domain.DTOs
{
    public class TranslationDto : BaseDto<long>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LanguageCode { get; set; }
    }
}