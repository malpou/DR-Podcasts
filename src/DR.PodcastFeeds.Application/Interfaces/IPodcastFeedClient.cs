namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastFeedClient
{
    Task<Domain.Podcast?> GetPodcast(string name);
}