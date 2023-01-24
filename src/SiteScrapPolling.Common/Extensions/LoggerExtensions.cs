namespace SiteScrapPolling.Common.Extensions;

public static class LoggerExtensions
{
    public static string ToFoundState(this bool condition) => condition ? "found" : "not found";
}
