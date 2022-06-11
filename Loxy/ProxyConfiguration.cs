namespace Loxy;

public class ProxyConfiguration
{
    private Uri? _parsedUri;

    /// <summary>
    ///     The gemini URI to proxy requests to
    /// </summary>
    public string RemoteUri { get; set; }

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

    public bool ServeFiles => !string.IsNullOrWhiteSpace(ContentRoot);

    public Uri ParsedUri
    {
        get
        {
            if (_parsedUri == null)
                _parsedUri = new Uri(RemoteUri);
            return _parsedUri;
        }
    }
}