using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public interface IPodcastReadStore
{
    Task<bool> Exists(string name);
    Task<Podcast> Get(string name, bool includeEpisodes = false);
}