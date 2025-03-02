namespace Application.Interfaces;

public interface IBuilder<TCommand, TResult>
{
    TResult Apply(TCommand command);
}
