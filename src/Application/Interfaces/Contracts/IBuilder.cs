namespace Application.Interfaces;

public interface IBuilder<in TCommand, out TResult>
{
    TResult Apply(TCommand command);
}