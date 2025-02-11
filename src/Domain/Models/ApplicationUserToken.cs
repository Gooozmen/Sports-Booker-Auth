using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
}
