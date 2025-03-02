using Shared.Interfaces;

namespace Application.Interfaces;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    Task<TResult> Handle(TCommand command);
}