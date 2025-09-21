using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CourtBooker.Auth.Infrastructure.Database;

// in Infrastructure project
public class DesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Staging.json", optional: false, reloadOnChange: true)
            .Build();

        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(config.GetConnectionString("AuthDb"))
            .Options;

        return new ApplicationDbContext(opts);
    }
}
