using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Clients;
using DR.PodcastFeeds.Infrastructure.Stores.Read;
using DR.PodcastFeeds.Infrastructure.Stores.Write;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Infrastructure.Managers;

internal class PodcastManager(
        IPodcastFeedClient podcastClient, 
        IPodcastReadStore podcastReadStore,
        IPodcastWriteStore podcastWriteStore,
        ILogger<PodcastManager> logger) 
    : IPodcastManager
{
    public async Task<bool> ValidateName(string name)
    {
        var exists = await podcastReadStore.Exists(name);
        
        if (exists)
        {
            logger.LogWarning("Podcast ({PodcastName}) already exists", name);
            
            return false;
        }
        
        var podcastFeed = await podcastClient.GetPodcastFeed(name);
        
        if (podcastFeed == null)
        {
            logger.LogWarning("Podcast ({PodcastName}) does not exist", name);
            
            return false;
        }
        
        logger.LogInformation("Podcast ({PodcastName}) is valid", name);
        
        return true;
    }

    public async Task AddPodcast(string name)
    {
        var podcastFeed = await podcastClient.GetPodcastFeed(name);
        
        if (podcastFeed == null)
        {
            logger.LogCritical("Podcast ({PodcastName}) does not exist in the system", name);
            
            return; 
        }
        
        var podcast = new Podcast(
            title: podcastFeed.Title,
            name: name,
            description: podcastFeed.Description,
            link: podcastFeed.Link,
            imageUrl: podcastFeed.Image.Url,
            category: new Category(podcastFeed.Category),
            episodes: podcastFeed.Items.Select(episode => new Episode(
                id: episode.Id,
                title: episode.Title,
                description: episode.Description,
                duration: episode.Duration,
                publishingDate: DateTime.Parse(episode.PubDate)
            ))
        );
        
        await podcastWriteStore.Add(podcast);
        
        logger.LogInformation("Podcast ({PodcastName}) added to system", name);
    }

    public Task RemovePodcast(string name)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePodcasts()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePodcast(string name)
    {
        throw new NotImplementedException();
    }
}