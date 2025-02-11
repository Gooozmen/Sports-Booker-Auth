using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;

namespace Domain.Models;

public class ApplicationUser : IdentityUser<Guid>,IActive
{
    public bool Active { get; set; }
}

