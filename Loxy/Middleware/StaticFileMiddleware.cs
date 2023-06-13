using Loxy.Configuration;

namespace Loxy.Middleware;

public class StaticFileMiddleware
{
    private readonly ProxyConfiguration _config;
    private readonly RequestDelegate _next;

    public StaticFileMiddleware(RequestDelegate next, ProxyConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Path.StartsWithSegments(Constants.StaticRoutePrefix,
                StringComparison.InvariantCultureIgnoreCase))
        {
            var path = Path.Combine(
                Path.GetFullPath(_config.ContentRoot),
                ctx.Request.Path.Value?[(Constants.StaticRoutePrefix.Length + 1)..] ?? string.Empty);

            if (!File.Exists(path))
            {
                ctx.Response.StatusCode = StatusCodes.Status404NotFound;
                return Task.CompletedTask;
            }

            return ctx.Response.SendFileAsync(path);
        }

        return _next(ctx);
    }
}