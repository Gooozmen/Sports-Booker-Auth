using System.Data;
using CourtBooker.Auth.Infrastructure.Database.Seeders;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace CourtBooker.Auth.Infrastructure.Database;

public class ApplicationDbContextInitializer : IContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly EntityFrameworkOption _entityFrameworkOption;
    private readonly IEnumerable<ISeeder> _seeders;
    private readonly ILogger _logger;

    public ApplicationDbContextInitializer
    (
        ApplicationDbContext context,
        IOptions<EntityFrameworkOption> option,
        IEnumerable<ISeeder> seeders,
        ILogger<ApplicationDbContextInitializer> logger
    )
    {
        _context = context;
        _seeders = seeders;
        _entityFrameworkOption = option.Value;
        _logger = logger;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_entityFrameworkOption.ExecuteRebuild && IsPgSql())
            {
                
                await _context.Database.CanConnectAsync();
                await ExecuteSeedAsync();
            }
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
            _logger.LogInformation("Dropping Schema ...");
            await _context.Database.ExecuteSqlRawAsync($"DROP SCHEMA IF EXISTS {DatabaseConstants.IdentitySchema} CASCADE;");
        }
        catch (Exception e)
        {
            _logger.LogError("Drop Identity Schema Failed - {0}",e.Message );
            throw new Exception(e.Message);
        }
    }

    private async Task ExecuteDatabaseBuildAsync()
    {
        try
        {
            _logger.LogInformation("Creating Schema ...");
            await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA {DatabaseConstants.IdentitySchema};");
            if(!await SchemaExistsAsync(DatabaseConstants.IdentitySchema))
                throw new Exception("Identity Schema does not exist");
                
            
        }
        catch (Exception e)
        {
            _logger.LogError("Create Identity Schema Failed - {0}",e.Message );
            throw new Exception(e.Message);
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

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new DataException(e.Message);
        }
        
    }
    
    public async Task<bool> SchemaExistsAsync(string schemaName)
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT EXISTS (
            SELECT 1
            FROM information_schema.schemata
            WHERE schema_name = @schema
        );";
        var param = command.CreateParameter();
        param.ParameterName = "@schema";
        param.Value = schemaName;
        command.Parameters.Add(param);

        var result = await command.ExecuteScalarAsync();
        return result is bool exists && exists;
    }

}

public interface IContextInitializer
{
    Task InitialiseAsync();
}