using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OL.Infrastructure;

namespace EA.Migrator;

public class OLDBContextFactory : IDesignTimeDbContextFactory<OLDbContext>
{
    private readonly string _connectionString;
    public OLDBContextFactory()
    {
        var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();
        var configuration = builder.Build();

        _connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public OLDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OLDbContext>();

        optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly(System.Reflection.Assembly.GetExecutingAssembly().FullName));

        return new OLDbContext(optionsBuilder.Options);
    }
}