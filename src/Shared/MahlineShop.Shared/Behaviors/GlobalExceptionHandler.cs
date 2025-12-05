using MahlineShop.Shared.Result;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MahlineShop.Shared.Behaviors;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"Exception occured: {exception.Message}");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = "An unexpected error occured."
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = Result.Result.Failure(Error.InternalServerError);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
