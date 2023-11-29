namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastReceiver
{
    Task<PodcastFeeds.Domain.Podcast> GetPodcastWithEpisodes(string name, int? last = null);
    Task<IEnumerable<PodcastFeeds.Domain.Podcast>> GetPodcasts(string? category = null);
}