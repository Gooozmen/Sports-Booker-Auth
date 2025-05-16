using CourtBooker.Auth.Infrastructure.Database.Seeders;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Infrastructure.Database;

public class ApplicationDbContextInitializer : IContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly EntityFrameworkOption _entityFrameworkOption;
    private readonly IEnumerable<ISeeder> _seeders;

    public ApplicationDbContextInitializer
    (
        ApplicationDbContext context,
        IOptions<EntityFrameworkOption> option,
        IEnumerable<ISeeder> seeders)
    {
        _context = context;
        _seeders = seeders;
        _entityFrameworkOption = option.Value;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_entityFrameworkOption.ExecuteRebuild && IsPgSql())
            {
                await ExecuteDatabaseDropAsync();
                await ExecuteDatabaseBuildAsync();
            }

            // await _context.Database.MigrateAsync();

            if (_entityFrameworkOption.ExecuteRebuild)
                await ExecuteSeedAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task ExecuteDatabaseDropAsync()
    {
        try
        {
            // await _context.Database.EnsureDeletedAsync();
            await _context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS Identity CASCADE;");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ExecuteDatabaseBuildAsync()
    {
        try
        {
            // await _context.Database.EnsureCreatedAsync();
            await _context.Database.ExecuteSqlRawAsync("CREATE SCHEMA Identity;");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private bool IsPgSql()
    {
        return _context.Database.IsNpgsql();
    }

    private async Task ExecuteSeedAsync()
    {
        var applicationUserSeeder =
            _seeders.FirstOrDefault(seeder => seeder.GetType() == typeof(ApplicationUserSeeder));
        if (applicationUserSeeder != null) await applicationUserSeeder.SeedAsync();
        var applicationRoleSeeder =
            _seeders.FirstOrDefault(seeder => seeder.GetType() == typeof(ApplicationRoleSeeder));
        if (applicationRoleSeeder != null) await applicationRoleSeeder.SeedAsync();

        await _context.SaveChangesAsync();
    }
}

public interface IContextInitializer
{
    Task InitialiseAsync();
}