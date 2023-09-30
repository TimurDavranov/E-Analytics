using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace EA.Domain.Events
{
    public class EditCategoryEvent : BaseEvent
    {
        public EditCategoryEvent() : base(nameof(EditCategoryEvent))
        {
        }
        public long CategoryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}