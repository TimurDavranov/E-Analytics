using EA.Domain.Primitives.Base;

namespace EA.Infrastructure.Commands.Categories
{
    public class RemoveCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
    }
}