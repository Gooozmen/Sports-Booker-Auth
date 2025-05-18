using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Domain.Models;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
}