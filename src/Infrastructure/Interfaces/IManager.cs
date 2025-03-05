
using Shared.Abstractions;

namespace Infrastructure.Interfaces;
public interface ICommandManager<in TModel, TResult>
{
    Task<TResult> CreateAsync(TModel model);
}

public interface IQueryableManager<TResult,in TData> where TData : PropertyTypeBase
{
    Task<TResult?> GetAsync(TData data);
}