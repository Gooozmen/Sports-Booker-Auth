using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>, IActive
{
    public bool Active { get; set; }
}