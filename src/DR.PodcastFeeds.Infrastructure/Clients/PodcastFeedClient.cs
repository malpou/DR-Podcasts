using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Extensions;

namespace DR.PodcastFeeds.Infrastructure.Clients;

public class PodcastFeedClient(HttpClient httpClient) : IPodcastFeedClient
{
    public async Task<bool> PodcastExists(string name)
    {
        var response = await GetRssFeed(name);
        return response.IsSuccessStatusCode;
    }

    public async Task<Podcast?> GetPodcast(string name)
    {
        var response = await GetRssFeed(name);

        if (!response.IsSuccessStatusCode) return null;

        var stream = await response.Content.ReadAsStreamAsync();
        var podcastFeed = stream.DeserializeRssFeed();

        var podcast = new Podcast(
            podcastFeed.Title,
            name,
            podcastFeed.Description,
            podcastFeed.Link,
            podcastFeed.Image.Url,
            new Category(podcastFeed.Category),
            podcastFeed.Items.Select(episode => new Episode(
                episode.Id,
                episode.Title,
                episode.Description,
                episode.Duration,
                DateTime.Parse(episode.PubDate)
            ))
        );

        return podcast;
    }

    private async Task<HttpResponseMessage> GetRssFeed(string podcastName)
    {
        var url = $"https://api.dr.dk/podcasts/v1/feeds/{podcastName}.xml";
        return await httpClient.GetAsync(url);
    }
}