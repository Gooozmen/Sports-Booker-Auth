using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.Seeders;

public class ApplicationUserSeeder : ISeeder
{
    private readonly List<string> _seedPasswords;
    private readonly List<ApplicationUser> _seedUsers;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserSeeder(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _seedUsers = GetSeedUsers();
        _seedPasswords = GetSeedPasswords();
    }

    public async Task SeedAsync()
    {
        var limit = _seedUsers.Count();

        for (var i = 0; i < limit; i++)
        {
            var result = await _userManager.CreateAsync(_seedUsers[i], _seedPasswords[i]);
        }
    }

    private List<string> GetSeedPasswords()
    {
        return new List<string>
        {
            "Ab1@xTy9", //1
            "Xz2#LmPq7",
            "Ef4!GhJk8",
            "Mn5$QwEr3",
            "Yt6*ZxCv2",
            "Tr7@VbNm1",
            "Op8#WxYz5",
            "Qw9!AsDf4",
            "Lk0$GhJm3",
            "Pz1*MnOp6" //10
        };
    }


    private List<ApplicationUser> GetSeedUsers()
    {
        return new HashSet<ApplicationUser>
        {
            new()
            {
                Id = Guid.Parse("32ea4559-8cff-4339-84b1-45b41f9cfaf9"), Email = "aclace0@mlb.com",
                UserName = "aclace0@mlb.com", Active = true
            },
            new()
            {
                Id = Guid.Parse("1b1637de-23d3-4f66-b4b4-4a9d169ad474"), Email = "covendale1@t-online.de",
                UserName = "covendale1@t-online.de", Active = true
            },
            new()
            {
                Id = Guid.Parse("5aa50755-fb15-46a7-94f2-563bfb53f0a7"), Email = "jnaseby2@harvard.edu",
                UserName = "jnaseby2@harvard.edu", Active = true
            },
            new()
            {
                Id = Guid.Parse("a239c2fc-b2b3-4d49-8a18-95d0225ef816"), Email = "abosma3@craigslist.org",
                UserName = "abosma3@craigslist.org", Active = true
            },
            new()
            {
                Id = Guid.Parse("17cc2737-c9f2-424b-9818-da2a12f823b9"), Email = "fbroome4@springer.com",
                UserName = "fbroome4@springer.com", Active = true
            },
            new()
            {
                Id = Guid.Parse("25fad734-94de-4fcb-9cff-818fee01da3f"), Email = "jwragge5@quantcast.com",
                UserName = "jwragge5@quantcast.com", Active = true
            },
            new()
            {
                Id = Guid.Parse("48260a83-70d6-40ea-a9fb-045de7d9b8a4"), Email = "ddurnin6@nsw.gov.au",
                UserName = "ddurnin6@nsw.gov.au", Active = true
            },
            new()
            {
                Id = Guid.Parse("49a27144-f003-4a94-bbe8-ee88de4729b4"), Email = "dobington7@walmart.com",
                UserName = "dobington7@walmart.com", Active = false
            },
            new()
            {
                Id = Guid.Parse("b6648198-b838-472e-bb08-fb5dd7b079b3"), Email = "apynner8@wordpress.org",
                UserName = "apynner8@wordpress.org", Active = false
            },
            new()
            {
                Id = Guid.Parse("1963767f-2bed-464a-bdd6-65c9bd8ee2f5"), Email = "rcomini9@friendfeed.com",
                UserName = "rcomini9@friendfeed.com", Active = false
            }
        }.ToList();
    }
}