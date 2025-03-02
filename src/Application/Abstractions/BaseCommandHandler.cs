using Application.Interfaces;
using Shared.Interfaces;

namespace Application.Abstractions;

public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand, object> where TCommand : ICommand
{
    public abstract Task<Object> Handle(TCommand command);
}
