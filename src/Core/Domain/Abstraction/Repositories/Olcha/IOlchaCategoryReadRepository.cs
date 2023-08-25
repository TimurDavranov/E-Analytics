using Domain.Entities.Olcha;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repositories.Olcha
{
    public interface IOlchaCategoryReadRepository
    {
        Task<OlchaCategory?> GetByOlchaIdAsync(long id, Func<IQueryable<OlchaCategory>, IIncludableQueryable<OlchaCategory, object>>? include = null);
    }
}
