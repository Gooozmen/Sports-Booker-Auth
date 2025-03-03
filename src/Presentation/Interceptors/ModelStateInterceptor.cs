using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Responses;
using Shared.Enums;

namespace Presentation.Interceptors;

public class ModelStateInterceptor : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var failedResponse = new ControllerResponse<string[]>
            {
                IsSuccess = false,
                Data = errors,
                Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCodes.ModelStateInvalid),
                StatusCode = (int)HttpStatusCodes.ModelStateInvalid
            };
            
            context.Result = new BadRequestObjectResult(failedResponse);
        }
    }
}

