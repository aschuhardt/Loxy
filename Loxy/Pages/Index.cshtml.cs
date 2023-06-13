using System.Net;
using Loxy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Opal;
using Opal.Document.Line;
using Opal.Response;

namespace Loxy.Pages;

public class ProxyPageModel : PageModel
{
    private readonly ProxyConfiguration _config;
    private readonly ILogger<ProxyPageModel> _logger;
    private readonly IOpalClient _opalClient;

    public ProxyPageModel(ILogger<ProxyPageModel> logger, IOpalClient opalClient, ProxyConfiguration config)
    {
        _logger = logger;
        _opalClient = opalClient;
        _config = config;
    }

    public IEnumerable<ILine> Lines { get; private set; }
    public LineRenderer LineRenderer { get; private set; }

    private static bool IsExternalUri(PathString path)
    {
        return path.StartsWithSegments(Constants.ExternalRoutePrefix);
    }

    private static string ParseExternalUri(string path)
    {
        // + 1 to skip the trailing slash after "/ext"
        return Uri.UnescapeDataString(path[(Constants.ExternalRoutePrefix.Length + 1)..]);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(_config.RemoteUri))
            return RedirectToPage("Loxy");

        if (_config.ServeFiles)
        {
            ViewData["Javascript"] = _config.Javascript;
            ViewData["Stylesheet"] = _config.Stylesheet;
        }

        if (_config.HasHeadInsertContent)
        {
            var path = Path.Combine(_config.ContentRoot, _config.HeadInsert);
            ViewData["HeadInsert"] = await System.IO.File.ReadAllTextAsync(path);
        }

        string uri;

        try
        {
            uri = IsExternalUri(Request.Path)
                ? ParseExternalUri(Request.Path)
                : $"{_config.RemoteUri}{Request.Path}"; // TODO: this isn't great

            if (string.IsNullOrWhiteSpace(uri))
                return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Exception thrown while attempting to parse a request path: {Message}", e.Message);
            return BadRequest();
        }

        try
        {
            LineRenderer = new LineRenderer(_config, HttpContext);

            if (Request.Query.Any())
                uri = $"{uri}{Request.QueryString}";

            if (string.IsNullOrWhiteSpace(uri))
                uri = _config.RemoteUri;

            var response = await _opalClient.SendRequestAsync(uri);

            _logger.LogInformation("({IP}) {URI} {Response}", HttpContext.Connection.RemoteIpAddress, uri,
                response.ToString());

            if (response is ErrorResponse error)
            {
                switch (error.Status)
                {
                    case Opal.Response.StatusCode.Unknown:
                    case Opal.Response.StatusCode.ServerUnavailable:
                    case Opal.Response.StatusCode.CgiError:
                    case Opal.Response.StatusCode.ProxyError:
                    case Opal.Response.StatusCode.SlowDown:
                    case Opal.Response.StatusCode.PermanentFailure:
                    case Opal.Response.StatusCode.ProxyRequestRefused:
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            response.ToString() ?? string.Empty);
                    case Opal.Response.StatusCode.NotFound:
                        return NotFound();
                    case Opal.Response.StatusCode.Gone:
                        return StatusCode(StatusCodes.Status410Gone);
                    case Opal.Response.StatusCode.BadRequest:
                        return BadRequest(error.Message);
                    case Opal.Response.StatusCode.ClientCertificateRequired:
                        return Forbid();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (response is InputRequiredResponse input)
            {
                return Redirect(Url.Page("Prompt",
                    new { returnUrl = Url.Page("Index"), message = input.Message ?? string.Empty }) ?? string.Empty);
            }

            if (response is SuccessfulResponse success)
            {
                if (success is GemtextResponse gmi)
                {
                    Lines = gmi.AsDocument().ToList();

                    ViewData["Title"] = Lines.Where(l => l.LineType == LineType.Heading).Cast<TextualLineBase>()
                        .FirstOrDefault()?.Text;

                    return Page();
                }

                // the body stream must be copied to a fresh stream because its position is set to the end of the request URL
                var resultStream = new MemoryStream();
                await success.Body.CopyToAsync(resultStream);

                resultStream.Seek(0, SeekOrigin.Begin);
                return File(resultStream, success.MimeType);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception thrown while serving a request for {URI}: {Message}", uri, e.Message);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        return NotFound();
    }
}