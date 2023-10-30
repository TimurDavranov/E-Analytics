using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace OL.Infrastructure.Commands.Categories
{
    public class AddOLCategoryCommand : BaseCommand
    {
        public long SystemId { get; set; }
        public long? ParentId { get; set; }
        public string NameRu { get; set; }
        public string NameUz { get; set; }
        public string NameOz { get; set; }
        public string NameEn { get; set; }
    }
}