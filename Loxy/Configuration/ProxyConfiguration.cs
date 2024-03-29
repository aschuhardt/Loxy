﻿namespace Loxy.Configuration;

public class ProxyConfiguration
{
    private const string GeminiSchemePrefix = $"{Constants.GeminiScheme}://";
    private Uri _parsedUri;
    private string _remoteUri;

    /// <summary>
    ///     The gemini URI to proxy requests to
    /// </summary>
    public string RemoteUri
    {
        get => _remoteUri;
        set
        {
            _remoteUri = value;
            _parsedUri = null;
            if (!string.IsNullOrWhiteSpace(_remoteUri) && !_remoteUri.StartsWith(GeminiSchemePrefix))
                _remoteUri = $"{GeminiSchemePrefix}{_remoteUri}";
        }
    }

    /// <summary>
    ///     Path to a custom stylesheet to use
    /// </summary>
    public string Stylesheet { get; set; }

    /// <summary>
    ///     Root path to serve files from.  If null, no static files will be served.
    /// </summary>
    public string ContentRoot { get; set; }

    /// <summary>
    ///     Path to a custom javascript file to include in proxied pages
    /// </summary>
    public string Javascript { get; set; }

    /// <summary>
    ///     The port to listen to on localhost
    /// </summary>
    public int Port { get; set; } = 8080;

    /// <summary>
    ///     Path to an HTML file to insert into the head section of each page
    /// </summary>
    public string HeadInsert { get; set; }

    /// <summary>
    ///     Flag which disables the /lxy Loxy build information path
    /// </summary>
    public bool NoLoxyInfo { get; set; }

    /// <summary>
    ///     Flag which enables a footer on each page listing some request stats
    /// </summary>
    public bool IncludeStatsFooter { get; set; }

    public bool ServeFiles => !string.IsNullOrWhiteSpace(ContentRoot);

    public bool HasHeadInsertContent => ServeFiles && !string.IsNullOrWhiteSpace(HeadInsert);

    public Uri GetParsedUri()
    {
        if (_parsedUri == null && !string.IsNullOrWhiteSpace(RemoteUri))
            _parsedUri = new Uri(RemoteUri);

        return _parsedUri;
    }
}