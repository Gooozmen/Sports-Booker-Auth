namespace CourtBooker.Auth.Application.Interfaces;

public interface IFactory<in TSource, out TResponse, in TData>
{
    TResponse Create(TSource source, TData data);
    TResponse Create();
}

public interface IFactory<in TSource, out TResponse>
{
    TResponse Create(TSource source);
}