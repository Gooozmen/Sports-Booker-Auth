using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Queries;
using Shared.Wrappers;

namespace Application.Interfaces;

public interface IApplicationUserManager :
    ICommandManager<ApplicationUserWrapper, IdentityResult>,
    IQueryableManager<ApplicationUser, UserQuery>
{
}