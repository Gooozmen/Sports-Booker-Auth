using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourtBooker.Auth.Infrastructure.Database;

public class ApplicationDbContext :
    IdentityDbContext
    <ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>,
    IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
    public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
    public virtual DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
    public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasAnnotation("Npgsql:DefaultSchema", "Identity");
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}