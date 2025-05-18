using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationUser : IdentityUser<Guid>, IActive
{
    public bool Active { get; set; }
}