using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Opal;
using Opal.Response;

namespace Loxy.Extensions;

public static class RazorPageExtensions
{
    public static IHtmlContent RenderStylesheet(this RazorPage<dynamic> page)
    {
        try
        {
            if (!page.ViewData.TryGetValue("Stylesheet", out var data) ||
                data is not string cssPath || string.IsNullOrEmpty(cssPath))
                return HtmlString.Empty;

            var builder = new TagBuilder("link");
            builder.Attributes.Add("rel", "stylesheet");
            builder.Attributes.Add("href", $"{Constants.StaticRoutePrefix}/{cssPath}");

            return builder.RenderSelfClosingTag();
        }
        catch (Exception)
        {
            return HtmlString.Empty;
        }
    }

    public static IHtmlContent RenderHeadInsert(this RazorPage<dynamic> page)
    {
        try
        {
            if (!page.ViewData.TryGetValue("HeadInsert", out var data) ||
                data is not string content || string.IsNullOrEmpty(content))
                return HtmlString.Empty;

            return new HtmlString(content);
        }
        catch (Exception)
        {
            return HtmlString.Empty;
        }
    }

    public static IHtmlContent RenderJavascript(this RazorPage<dynamic> page)
    {
        try
        {
            if (!page.ViewData.TryGetValue("Javascript", out var data) ||
                data is not string jsPath || string.IsNullOrEmpty(jsPath))
                return HtmlString.Empty;

            var builder = new TagBuilder("link");
            builder.Attributes.Add("src", $"{Constants.StaticRoutePrefix}/{jsPath}");

            return new HtmlContentBuilder(new List<object>
            {
                builder.RenderStartTag(),
                builder.RenderEndTag()
            });
        }
        catch (Exception)
        {
            return HtmlString.Empty;
        }
    }

    public static IHtmlContent RenderResponseStatsFooter(this RazorPage<dynamic> page)
    {
        try
        {
            if (!page.ViewData.TryGetValue("GeminiResponse", out var data) ||
                data is not IGeminiResponse response)
                return HtmlString.Empty;

            var elapsedSeconds = page.ViewData["ElapsedSeconds"] as double? ?? 0.0;

            var div = new TagBuilder("div");
            div.AddCssClass("stats-footer");

            var text = response switch
            {
                SuccessfulResponse success => $"{(int)success.Status} {success.MimeType} in {elapsedSeconds:F}s | {success.Uri}",
                ErrorResponse error => $"{error.Uri} | {(int)error.Status} {error.Message}",
                _ => response.ToString() ?? string.Empty
            };

            div.InnerHtml.Append(text);

            return div;
        }
        catch (Exception)
        {
            return HtmlString.Empty;
        }
    }
}