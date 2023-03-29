using System.Net;
using Vinstag.Common.Models.Exceptions;
using Vinstag.InstagramAPI.Exception;

namespace Vinstag.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (NoUsersException avEx)
        {
            _logger.LogError($"A new NoUsersException has been thrown: {avEx}");
            await HandleExceptionAsync(httpContext, avEx);
        }
        catch (NoSessionException avEx)
        {
            _logger.LogError($"A new NoSessionException has been thrown: {avEx}");
            await HandleExceptionAsync(httpContext, avEx);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var message = exception switch
        {
            NoUsersException => "No Users",
            NoSessionException => "No session data",
            _ => "Internal Server Error from the custom middleware."
        };

        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        }.ToString());
    }
}