using CourtBooker.Auth.Shared.Interfaces;

namespace CourtBooker.Auth.Application.Interfaces;

public interface IQueryableManager<TResult, in TData> where TData : IPropertyType
{
    Task<TResult?> GetAsync(TData data);
}