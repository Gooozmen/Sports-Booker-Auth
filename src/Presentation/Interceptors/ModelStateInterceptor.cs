using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Responses;

namespace CourtBooker.Auth.Presentation.Interceptors;

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

            var failedResponse = new Response<string[]>
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