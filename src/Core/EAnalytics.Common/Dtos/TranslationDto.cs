using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Primitives.DTOs;

namespace EAnalytics.Common.Dtos
{
    public class TranslationDto : BaseDto<long>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LanguageCode LanguageCode { get; set; }
    }
}