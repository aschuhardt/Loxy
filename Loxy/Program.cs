using Loxy;
using Opal;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
        ContentRootPath = Directory.GetCurrentDirectory(),
        WebRootPath = string.Empty,
        Args = args
    });

var config = builder.Configuration
    .GetSection("Proxy").Get<ProxyConfiguration>();

if (!string.IsNullOrWhiteSpace(config.ContentRoot))
    builder.Environment.ContentRootPath = config.ContentRoot;

builder.Services
    .AddRazorPages()
    .AddRazorPagesOptions(options => { options.Conventions.AddPageRoute("/index", "{*url}"); });

builder.Services
    .AddSingleton(config)
    .AddSingleton<IOpalClient>(_ => new OpalClient());

var app = builder.Build();

if (!string.IsNullOrWhiteSpace(config.ContentRoot))
    app.UseStaticFiles();

app.MapRazorPages();
app.Run();