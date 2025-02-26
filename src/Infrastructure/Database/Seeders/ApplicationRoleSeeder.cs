using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Database.Seeders;

public class ApplicationRoleSeeder : ISeeder
{
    private readonly ApplicationDbContext _db;
    private List<ApplicationRole> _seedRoles;

    public ApplicationRoleSeeder(ApplicationDbContext db)
    {
        _db = db;
        _seedRoles = GetSeedRoles();
    }

    private List<ApplicationRole> GetSeedRoles()
    {
        var dataSet = new HashSet<ApplicationRole>
        {
            new ApplicationRole 
                { Id = Guid.Parse("bc9c044a-8d6a-4426-b5ff-5157b0c45ce2"), Name = "Admin",  ConcurrencyStamp = Guid.NewGuid().ToString(), Active = true },
            new ApplicationRole 
                { Id = Guid.Parse("eb539578-ef11-4a0c-ba8e-5f1024e0cdc2"), Name = "User", ConcurrencyStamp = Guid.NewGuid().ToString(), Active = true },
            new ApplicationRole 
                { Id = Guid.Parse("f7fb952d-0c1c-4fa1-9bdb-c75b3f945e2a"), Name = "Coach", ConcurrencyStamp = Guid.NewGuid().ToString(), Active = true },
            new ApplicationRole 
                { Id = Guid.Parse("799f14d7-8b47-4ecf-b2c6-cf138feb0e7a"), Name = "Organizer", ConcurrencyStamp = Guid.NewGuid().ToString(), Active = true },
            new ApplicationRole 
                { Id = Guid.Parse("6c1b350f-0873-448f-b5ae-b5fc70c68c53"), Name = "Player", ConcurrencyStamp = Guid.NewGuid().ToString(), Active = true }
        };

        foreach (var set in dataSet) set.NormalizedName = set.Name?.ToUpper();
        
        return dataSet.ToList();
    }

    public async Task SeedAsync()
    {
        await _db.Roles.AddRangeAsync(_seedRoles);
        await _db.SaveChangesAsync();
    }
}   

