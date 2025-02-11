
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Seeds;

public class UserSeederService : ISeederService
{
    private readonly ApplicationDbContext _context;

    public UserSeederService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
    }
}