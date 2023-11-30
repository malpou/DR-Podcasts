namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastFeedClient
{
    Task<bool> PodcastExists(string name);
    Task<Domain.Podcast?> GetPodcast(string name);
}