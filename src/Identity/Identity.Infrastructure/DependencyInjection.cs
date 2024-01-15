using Microsoft.Extensions.Configuration;
using Identity.Data;
using Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            var configuration = services
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            
            

            return services;
        }
    }
}
