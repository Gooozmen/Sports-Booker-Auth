namespace Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    T GetManager<T>() where T : class;
}