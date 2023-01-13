namespace SiteScrapPolling.Scrapping;

public interface IScrapper
{
    public IAsyncEnumerable<ScrapResponse> ScrapAsync(ScrapRequest request, CancellationToken cancellationToken);
}
