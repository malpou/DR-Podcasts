using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class RemovePodcastCommandHandler(
    IPodcastReadStore podcastReadStore,
    IPodcastWriteStore podcastWriteStore,
    ILogger<RemovePodcastCommandHandler> logger
) : IRequestHandler<RemovePodcastCommand, (bool, string)>
{
    public async Task<(bool, string)> Handle(RemovePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcastName = request.Name;

        var exists = await podcastReadStore.PodcastsExists(podcastName);

        if (!exists)
        {
            logger.LogWarning("Podcast ({PodcastName}) does not exist in the system", podcastName);

            return (false, "Podcast does not exist in the system");
        }

        var removed = await podcastWriteStore.Remove(podcastName);

        if (removed)
        {
            logger.LogInformation("Podcast ({PodcastName}) removed from the system", podcastName);

            return (true, "Podcast removed from the system");
        }

        logger.LogCritical("Podcast ({PodcastName}) could not be removed from the system", podcastName);


        return (false, "Podcast could not be removed from the system");
    }
}