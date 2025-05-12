namespace CourtBooker.Auth.Infrastructure.Interfaces;

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}