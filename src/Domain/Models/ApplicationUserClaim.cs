using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationUserClaim : IdentityUserClaim<Guid>, IActive
{
    public bool Active { get; set; }
}