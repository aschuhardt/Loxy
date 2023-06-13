using System.Net;
using Loxy.Configuration;

namespace Loxy.Middleware;

public class HideLoxyInfoPageMiddleware
{
    private readonly ProxyConfiguration _config;
    private readonly RequestDelegate _next;

    public HideLoxyInfoPageMiddleware(RequestDelegate next, ProxyConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Path.StartsWithSegments("/lxy/overview", StringComparison.InvariantCultureIgnoreCase))
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Task.CompletedTask;
        }

        return _next(ctx);
    }
}