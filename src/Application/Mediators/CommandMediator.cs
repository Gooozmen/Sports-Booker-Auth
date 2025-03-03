using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;

namespace Application.Mediators;

public class CommandMediator(IServiceProvider serviceProvider) : ICommandMediator
{
    public async Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.Handle(command, cancellationToken);
    }
}