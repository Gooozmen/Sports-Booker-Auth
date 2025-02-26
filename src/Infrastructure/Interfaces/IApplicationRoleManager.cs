using System.Globalization;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IApplicationRoleManager
{
    Task<IdentityResult> CreateRoleAsync(ApplicationRole role);   
}