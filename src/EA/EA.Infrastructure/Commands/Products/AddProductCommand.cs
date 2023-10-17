using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Enums;

namespace EA.Infrastructure.Commands.Products;


public class AddProductCommand : BaseCommand
{
    [JsonInclude]
    public string Name { get; set; }
    
    [JsonInclude]
    public Guid CategoryId { get; set; }
    
    [JsonInclude]
    public decimal Price { get; set; }
    
    [JsonInclude]
    public string Url { get; set; }

    [JsonInclude] 
    public SystemName SystemName { get; set; }

}