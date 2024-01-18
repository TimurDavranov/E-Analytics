using EAnalytics.Common.Abstractions.Repositories;
using Identity.Data;
using Identity.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Services
{
    public class UserService(IRepository<ApplicationUser, IdentityContext> repository)
    {

        public ApplicationUser? GetUser(string id)
        {
            return repository.Get(x => x.Id == id);
        }

    }
}
