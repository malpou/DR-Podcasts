using DR.PodcastFeeds.Application.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public class AddPodcastCommandHandler(
    IPodcastManager podcastManager,
    ILogger<AddPodcastCommand> logger
) : IRequestHandler<AddPodcastCommand, (bool, string)>
{
    public async Task<(bool, string)> Handle(
        AddPodcastCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var podcastName = request.Name;

        var isValid = await podcastManager.ValidateName(podcastName);
        
        if (!isValid)
        {
            logger.LogWarning("Podcast name ({PodcastName}) is not valid or already exists", podcastName);
            
            return (false, "Podcast name  is not valid or already exists");
        }

        BackgroundJob.Enqueue<IPodcastManager>(x => x.AddPodcast(podcastName));
        
        logger.LogInformation("Podcast ({Podcast}) added to system", podcastName);
        
        return (true, "Podcast added to system");
    }
}