using MediatR;
using Microsoft.Extensions.Logging;
using CourtBooker.Auth.Shared.Interfaces;

namespace CourtBooker.Auth.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>
    (ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse
{
    public async Task<TResponse> Handle
        (
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken
        )
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling request: {@RequestName} " + "with data: {@request}", requestName, request);

        var response = await next();
        
        switch (response.IsSuccess)
        {
            case true:
                logger.LogInformation("Completed Successfully: {@RequestName} - {@Response}", requestName, response);
                break;
            case false:
                logger.LogWarning("Failure: {@RequestName} - {@Response}", requestName, response);
                break;
        }
        return response;
    }
}