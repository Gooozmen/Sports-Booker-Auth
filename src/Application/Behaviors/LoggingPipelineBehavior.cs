using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling request: {@RequestName} with data: {@request} at {@DateTime}", requestName, request, DateTime.Now );

        var response = await next();
        
        if(response.IsSuccess)
            logger.LogInformation("Completed Successfully: {@RequestName} - {@Response}", requestName, response);
        else if(!response.IsSuccess)
            logger.LogWarning("Failure: {@RequestName} - {@Response}", requestName, response);
        else 
            logger.LogDebug("Unknown result: {@RequestName} - {@Response}", requestName, response);
        
        return response;
    }
}