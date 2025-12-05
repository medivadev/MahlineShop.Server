using MahlineShop.Shared.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MahlineShop.Shared.Behaviors;

public class TrafficLoggingFilter(ILogger<TrafficLoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var method = context.HttpContext.Request.Method;
        var endpointName = context.HttpContext.GetEndpoint()?.DisplayName ?? "Unkown";

        var inputArgs = context.Arguments.FirstOrDefault(a => a is not HttpContext && a is not CancellationToken);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next(context);

            stopwatch.Stop();

            int statusCode = 200; // Default assumption

            if (result is IStatusCodeHttpResult httpResult && httpResult.StatusCode.HasValue)
            {
                statusCode = httpResult.StatusCode.Value;
            }

            // 3. LOGGING STRATEGY 🧠

            if (statusCode >= 500)
            {
                // Server Errors -> Log as Error
                logger.LogError(
                    "ENDPOINT: {EndpointName} [{Method}] | SERVER ERROR ({StatusCode}) | INPUT: {@Input} | TIME: {Elapsed}ms",
                    endpointName, method, statusCode, inputArgs ?? "None", stopwatch.ElapsedMilliseconds);
            }
            else if (statusCode >= 400)
            {
                // Business Failures (Validation, NotFound, etc.) -> Log as Warning
                // This ensures it goes to your 'ErrorLogs' table based on your Serilog config
                logger.LogWarning(
                    "ENDPOINT: {EndpointName} [{Method}] | BUSINESS FAILURE ({StatusCode}) | INPUT: {@Input} | OUTPUT: {@Output} | TIME: {Elapsed}ms",
                    endpointName, method, statusCode, inputArgs ?? "None", result, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                // Success (200, 201) -> Log as Information
                // This goes to your 'AccessLogs' table
                logger.LogInformation(
                    "ENDPOINT: {EndpointName} [{Method}] | SUCCESS ({StatusCode}) | INPUT: {@Input} | OUTPUT: {@Output} | TIME: {Elapsed}ms",
                    endpointName, method, statusCode, inputArgs ?? "None", result, stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            logger.LogError(
                "ENDPOINT: {EndpointName} [{Method}] | FAILED | INPUT: {@Input} | ERROR: {Message}",
                endpointName, method, inputArgs ?? "None", ex.Message);

            throw;
        }
    }
}
