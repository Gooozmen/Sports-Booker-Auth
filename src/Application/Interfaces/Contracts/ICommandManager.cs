namespace Application.Interfaces;

public interface ICommandManager<in TModel, TResult>
{
    Task<TResult> CreateAsync(TModel model);
    Task<TResult> UpdateAsync(TModel model);
}