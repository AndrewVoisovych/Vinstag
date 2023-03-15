using Microsoft.Extensions.Primitives;

namespace Vinstag.API.Middleware;

public class AccessTokenMiddleware
{
    private readonly RequestDelegate _next;
    public AccessTokenMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        //const string HeaderKeyName = "MiddlewareHeaderKey";
        //context.Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
        //if (context.Items.ContainsKey(HeaderKeyName))
        //{
        //    context.Items[HeaderKeyName] = headerValue;
        //}
        //else
        //{
        //    context.Items.Add(HeaderKeyName, $"{headerValue}-received");
        //}
        await _next(context);
    }
}