using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Queries;

namespace Application.Interfaces;

public interface IApplicationRoleManager :
    ICommandManager<ApplicationRole, IdentityResult>,
    IQueryableManager<ApplicationRole, ApplicationRoleQuery>
{
}