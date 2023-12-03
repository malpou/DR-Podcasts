namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastReadStore
{
    Task<bool> PodcastsExists(string name);
    Task<IEnumerable<Domain.Podcast>> GetAll(string? category = null);
}