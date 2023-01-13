using System.Runtime.CompilerServices;

namespace SiteScrapPolling.Scrapping.Scrappers;

public class DefaultScrapper : IScrapper
{
    public async IAsyncEnumerable<ScrapResponse> ScrapAsync(
        ScrapRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield break;
    }
}
