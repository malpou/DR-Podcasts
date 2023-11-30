namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastReadStore
{
    Task<bool> Exists(string name);
    Task<IEnumerable<Domain.Podcast>> GetAll(string? category = null);
}