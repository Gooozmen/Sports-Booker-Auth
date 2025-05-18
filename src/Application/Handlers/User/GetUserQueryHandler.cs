using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Application.Interfaces;
using MediatR;
using CourtBooker.Auth.Shared.Queries;
using CourtBooker.Auth.Shared.Responses.User;

namespace CourtBooker.Auth.Application.Handlers;

public class GetUserQueryHandler
    (
        IApplicationUserManager userManager,
        IApplicationUserBuilder builder
    ) 
    : IRequestHandler<UserQuery, UserResponse>
{
    public async Task<UserResponse> Handle(UserQuery request, CancellationToken cancellationToken = default)
    {
        var dataModel = await userManager.GetAsync(request);
        if (dataModel is null) return new UserResponse { IsSuccess = false };
        return builder.Apply(dataModel);
    }
}