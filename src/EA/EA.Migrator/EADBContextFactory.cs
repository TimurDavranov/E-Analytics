using EA.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EA.Migrator;

public class EADBContextFactory : IDesignTimeDbContextFactory<EADbContext>
{
    private readonly string _connectionString;
    public EADBContextFactory()
    {
        var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();
        var configuration = builder.Build();

        _connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public EADbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EADbContext>();

        optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly(System.Reflection.Assembly.GetExecutingAssembly().FullName));

        return new EADbContext(optionsBuilder.Options);
    }
}