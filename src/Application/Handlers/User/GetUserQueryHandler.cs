using Application.Builders;
using Application.Interfaces;
using MediatR;
using Shared.Queries;
using Shared.Responses.User;

namespace Application.Handlers;

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