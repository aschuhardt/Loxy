using Loxy;
using Loxy.Configuration;
using Loxy.Middleware;
using Opal;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
        ContentRootPath = Directory.GetCurrentDirectory(),
        Args = args
    });

builder.Configuration
    .AddCommandLine(args, Constants.ConfigurationKeyMap);

var config = builder.Configuration
    .GetSection("Proxy").Get<ProxyConfiguration>() 
             ?? throw new Exception("Failed to load configuration!");

if (config.ServeFiles)
{
    var path = Path.GetFullPath(config.ContentRoot);
    if (!Directory.Exists(path))
        throw new FileNotFoundException($"The content root path {path} does not exist");
    builder.Environment.ContentRootPath = path;
}

builder.Services
    .AddHttpContextAccessor()
    .AddSingleton(config)
    .AddSingleton<IOpalClient>(_ => new OpalClient())
    .AddRouting(options => options.LowercaseUrls = true)
    .AddRazorPages(options => options.Conventions.AddPageRoute("/index", "{*url}"));

var app = builder.Build();

app.Urls.Clear();
app.Urls.Add(new UriBuilder($"http://0.0.0.0:{config.Port}").Uri.ToString());

if (config.NoLoxyInfo)
    app.UseMiddleware<HideLoxyInfoPageMiddleware>();

if (config.ServeFiles)
    app.UseMiddleware<StaticFileMiddleware>();

app.MapRazorPages();

app.Run();