namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastReadStore
{
    Task<bool> Exists(string name);
    Task<Domain.Podcast> Get(string name, bool includeEpisodes = false);

    Task<IEnumerable<Domain.Podcast>> GetAll(bool includeEpisodes = false);
}