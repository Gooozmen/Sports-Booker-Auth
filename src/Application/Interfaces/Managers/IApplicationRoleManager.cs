using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using CourtBooker.Auth.Shared.Queries;

namespace CourtBooker.Auth.Application.Interfaces;

public interface IApplicationRoleManager : 
    ICommandManager<ApplicationRole,IdentityResult>,
    IQueryableManager<ApplicationRole, ApplicationRoleQuery>
{
}