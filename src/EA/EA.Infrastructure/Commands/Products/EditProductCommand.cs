using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;

namespace EA.Infrastructure.Commands.Products;


public class EditProductCommand : BaseCommand
{
    [JsonInclude]
    public long ProductId { get; set; }
    
    [JsonInclude]
    public string Name { get; set; }
    
    [JsonInclude]
    public decimal Price { get; set; }

    [JsonInclude] 
    public string ServiceName { get; set; }

}