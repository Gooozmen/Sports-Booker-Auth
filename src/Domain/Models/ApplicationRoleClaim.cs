using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>, IActive
{
    public bool Active { get; set; }
}