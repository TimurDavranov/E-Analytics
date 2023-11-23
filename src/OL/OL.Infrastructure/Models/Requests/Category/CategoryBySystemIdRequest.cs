using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL.Infrastructure.Models.Requests.Category;

public class CategoryBySystemIdRequest : BaseQuery
{
    public long SystemId { get; init; }
}

public class CategoryByIdRequest : BaseQuery
{
    public Guid Id { get; init; }
}

public class CategoryByNameRequest : BaseQuery
{
    public List<TranslationDto> Translations { get; set; }
}
