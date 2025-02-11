using Domain.Models;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid,ApplicationUserClaim, 
                                                      ApplicationUserRole, ApplicationUserLogin,
                                                      ApplicationRoleClaim, ApplicationUserToken>, 
                                                      IDbContext
{
    private readonly DbChangesInterceptor _interceptor;
    
    public virtual DbSet<AuditLog> Audits { get; set; }
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
    public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
    public virtual DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
    public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DbChangesInterceptor? interceptor = null) 
        : base(options)
    {
        _interceptor = interceptor;
    }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);
        builder.HasAnnotation("Npgsql:DefaultSchema", "Identity");
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Debugging: Print all applied entity types and schemas
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            Console.WriteLine($"Entity: {entityType.DisplayName()}, Table: {entityType.GetTableName()}, Schema: {entityType.GetSchema()}");
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.AddInterceptors(_interceptor);
}

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}