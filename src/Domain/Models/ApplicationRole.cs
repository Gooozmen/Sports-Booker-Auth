using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationRole : IdentityRole<Guid>, IActive
{
    public bool Active { get; set; }
}
