using Shared.Interfaces;

namespace Application.Interfaces;

public interface ICommandMediator
{
    Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand;

}