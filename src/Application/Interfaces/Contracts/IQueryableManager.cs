using Shared.Interfaces;

namespace Application.Interfaces;

public interface IQueryableManager<TResult, in TData> where TData : IPropertyType
{
    Task<TResult?> GetAsync(TData data);
}