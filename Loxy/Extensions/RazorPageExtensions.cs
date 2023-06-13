using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

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
}