using CourtBooker.Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Domain.Models;

public class ApplicationRole : IdentityRole<Guid>, IActive
{
    public bool Active { get; set; }
}