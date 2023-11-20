using EAnalytics.Common.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL.Infrastructure.Models.Requests.Category;

public class CategoryBySystemIdRequest : BaseQuery
{
    public long SystemId { get; private set; }
}

public class CategoryByIdRequest : BaseQuery
{
    public Guid Id { get; private set; }
}
