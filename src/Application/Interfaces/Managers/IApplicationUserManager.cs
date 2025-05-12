using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using CourtBooker.Auth.Shared.Queries;
using CourtBooker.Auth.Shared.Wrappers;

namespace CourtBooker.Auth.Application.Interfaces;

public interface IApplicationUserManager :
    ICommandManager<ApplicationUserWrapper, IdentityResult>,
    IQueryableManager<ApplicationUser, UserQuery>
{
}