using Infrastructure.Database.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ApplicationDbContextInitializer : IContextInitializer
{

    private const bool _executeRecreate = true;
    private const bool _executeSeeds = true;
    private readonly ApplicationDbContext _context;
    private readonly IEnumerable<ISeeder> _seeders;

    public ApplicationDbContextInitializer
    (
        ApplicationDbContext context,
        IEnumerable<ISeeder> seeders)
    {
        _context = context;
        _seeders = seeders;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_executeRecreate && IsPgSql())
            {
                await ExecuteDatabaseDropAsync();
                await ExecuteDatabaseBuildAsync();
            }
            
            // await _context.Database.MigrateAsync();

            if (_executeSeeds)
                await ExecuteSeedAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task ExecuteDatabaseDropAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
    
    private async Task ExecuteDatabaseBuildAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    private bool IsPgSql()
    {
        return _context.Database.IsNpgsql();
    }

    private async Task ExecuteSeedAsync()
    {
        var applicationUserSeeder = _seeders.FirstOrDefault(seeder => seeder.GetType() == typeof(ApplicationUserSeeder));
        if (applicationUserSeeder != null) await applicationUserSeeder.SeedAsync();
        
        await _context.SaveChangesAsync();
    }
}

public interface IContextInitializer
{
    Task InitialiseAsync();
}
