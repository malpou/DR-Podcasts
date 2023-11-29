using DR.PodcastFeeds.Infrastructure.Extensions;

namespace DR.PodcastFeeds.Infrastructure.Clients;

public class PodcastFeedClient(HttpClient httpClient) : IPodcastFeedClient
{
    public async Task<bool> PodcastExists(string name)
    {
        var response = await GetRssFeed(name);
        return response.IsSuccessStatusCode;
    }

    public async Task<Channel?> GetPodcastFeed(string name)
    {
        var response = await GetRssFeed(name);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var stream = await response.Content.ReadAsStreamAsync();
        
        return stream.DeserializeRssFeed();
    }
    
    private async Task<HttpResponseMessage> GetRssFeed(string podcastName)
    {
        var url = $"https://api.dr.dk/podcasts/v1/feeds/{podcastName}.xml";
        return await httpClient.GetAsync(url);
    }
}