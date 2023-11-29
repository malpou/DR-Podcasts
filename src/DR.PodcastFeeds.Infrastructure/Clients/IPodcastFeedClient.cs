using DR.PodcastFeeds.Infrastructure.Extensions;

namespace DR.PodcastFeeds.Infrastructure.Clients;

public interface IPodcastFeedClient
{
    Task<bool> PodcastExists(string name);
    Task<Channel?> GetPodcastFeed(string name);
}