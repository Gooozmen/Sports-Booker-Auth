using Infrastructure.Interfaces;
namespace Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _managers = new();

    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetManager<T>() where T : class
    {
        if (_managers.ContainsKey(typeof(T)))
        {
            return (T)_managers[typeof(T)];
        }

        var manager = _serviceProvider.GetRequiredService<T>();
        _managers[typeof(T)] = manager;
        return manager;
    }

    public void Dispose()
    {
        _managers.Clear();
    }
}
