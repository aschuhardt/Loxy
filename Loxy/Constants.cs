namespace Loxy;

public static class Constants
{
    public const string ExternalRoutePrefix = "/ext";
    public const string StaticRoutePrefix = "/sta";
    public const string GeminiScheme = "gemini";

    public static readonly Dictionary<string, string> ConfigurationKeyMap = new()
    {
        { "-u", "Proxy:RemoteUri" },
        { "--remote", "Proxy:RemoteUri" },
        { "-r", "Proxy:ContentRoot" },
        { "--root", "Proxy:ContentRoot" },
        { "-s", "Proxy:Stylesheet" },
        { "--stylesheet", "Proxy:Stylesheet" },
        { "-j", "Proxy:Javascript" },
        { "--script", "Proxy:Javascript" },
        { "-p", "Proxy:Port" },
        { "--port", "Proxy:Port" }
    };
}