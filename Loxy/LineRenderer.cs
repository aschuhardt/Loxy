using Loxy.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Opal.Document.Line;

namespace Loxy;

public class LineRenderer
{
    private readonly ProxyConfiguration _config;
    private readonly int _port;
    private readonly string _requestHost;
    private readonly string _scheme;

    public LineRenderer(ProxyConfiguration config, HttpContext httpContext)
    {
        _config = config;
        _scheme = httpContext.Request.Scheme;
        _requestHost = httpContext.Request.Host.Host;
        _port = httpContext.Request.Host.Port.GetValueOrDefault(-1);
    }

    private static IHtmlContent WrapInDivBlock(IHtmlContent content)
    {
        var builder = new TagBuilder("div");
        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                content,
                builder.RenderEndTag()
            });
    }

    private static IHtmlContent RenderHeadingLine(HeadingLine line)
    {
        var builder = new TagBuilder($"h{line.Level}");
        builder.InnerHtml.Append(line.Text);

        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderBody() ?? new HtmlString(null),
                builder.RenderEndTag()
            });
    }

    private string BuildHref(Uri uri)
    {
        if (uri.Scheme != Constants.GeminiScheme)
            return uri.ToString(); // leave non-gemini URIs as-is

        var builder = new UriBuilder(uri)
        {
            Scheme = _scheme,
            Port = _port,
            Host = _requestHost
        };

        if (uri.Host != _config.GetParsedUri().Host)
        {
            var trimmedUri = uri.ToString().Replace($"{Constants.GeminiScheme}://", string.Empty);
            builder.Path = $"{Constants.ExternalRoutePrefix}/{Uri.EscapeDataString(trimmedUri)}";
        }

        return builder.Uri.ToString();
    }

    private IHtmlContent RenderLinkLine(LinkLine line)
    {
        var builder = new TagBuilder("a");
        builder.Attributes.Add("href", BuildHref(line.Uri));
        builder.InnerHtml.Append(line.Text);

        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderBody() ?? new HtmlString(BuildHref(line.Uri)),
                builder.RenderEndTag()
            });
    }

    private static IHtmlContent RenderListLine(TextualLineBase line)
    {
        var builder = new TagBuilder("li");
        builder.InnerHtml.Append(line.Text);

        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderBody() ?? new HtmlString(null),
                builder.RenderEndTag()
            });
    }

    private static IHtmlContent RenderQuoteLine(TextualLineBase line)
    {
        var builder = new TagBuilder("blockquote");
        builder.InnerHtml.Append(line.Text);

        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderBody() ?? new HtmlString(null),
                builder.RenderEndTag()
            });
    }

    private static IHtmlContent RenderTextLine(TextualLineBase line)
    {
        var builder = new TagBuilder("p");
        builder.InnerHtml.Append(line.Text);

        return new HtmlContentBuilder(
            new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderBody() ?? new HtmlString(null),
                builder.RenderEndTag()
            });
    }

    public IHtmlContent RenderLine(ILine line)
    {
        return line switch
        {
            EmptyLine => HtmlString.Empty,
            FormattedBeginLine => new TagBuilder("pre").RenderStartTag(),
            FormattedEndLine => new TagBuilder("pre").RenderEndTag(),
            FormattedLine formattedLine => new HtmlString(formattedLine.Text),
            HeadingLine headingLine => RenderHeadingLine(headingLine),
            LinkLine linkLine => WrapInDivBlock(RenderLinkLine(linkLine)),
            ListLine listLine => RenderListLine(listLine),
            QuoteLine quoteLine => RenderQuoteLine(quoteLine),
            TextLine textLine => RenderTextLine(textLine),
            _ => throw new ArgumentOutOfRangeException(nameof(line))
        };
    }
}