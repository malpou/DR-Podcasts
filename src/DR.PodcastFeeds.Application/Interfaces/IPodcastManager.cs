namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastManager
{
    Task<bool> ValidateName(string name);
    Task AddPodcast(string name);
    Task RemovePodcast(string name);
    Task UpdatePodcasts();
    Task UpdatePodcast(string name);
}