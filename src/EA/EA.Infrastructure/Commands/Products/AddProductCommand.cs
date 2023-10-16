using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;

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
    public string ServiceName { get; set; }

}