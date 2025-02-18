using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ApplicationDbContextInitializer : IContextInitializer
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            var executedrop = true;
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && executedrop)
            {
                // Drop tables in the correct order based on foreign key dependencies
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetUserRoles"); // Dependent on AspNetUsers and AspNetRoles
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetRoleClaims"); // Dependent on AspNetRoles
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetUserClaims"); // Dependent on AspNetUsers
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetUserLogins"); // Dependent on AspNetUsers
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetUserTokens"); // Dependent on AspNetUsers
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetRoles"); // Parent of AspNetRoleClaims and AspNetUserRoles
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS AspNetUsers"); // Parent of AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, and AspNetUserTokens
                
                if (_context.Database.IsNpgsql())
                    await _context.Database.MigrateAsync();
            }
            
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

public interface IContextInitializer
{
    Task InitialiseAsync();
}
