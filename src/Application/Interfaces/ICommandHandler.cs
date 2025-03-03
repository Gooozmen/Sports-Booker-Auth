using Shared.Interfaces;

namespace Application.Interfaces;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}