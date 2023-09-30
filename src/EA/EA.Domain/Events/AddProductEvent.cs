using EA.Domain.DTOs;
using EAnalytics.Common.Events;
using Microsoft.EntityFrameworkCore;

namespace EA.Domain.Events
{
    public class AddProductEvent : BaseEvent
    {
        public AddProductEvent() : base(nameof(AddProductEvent))
        {
        }

        public Guid ProductId { get; set; }
        public Guid ProductSystemId { get; set; }

        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public string ServiceName { get; set; }
        
        
    }
}