using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class ValidatePodcastNameCommandHandler(
    IPodcastReadStore podcastReadStore,
    IPodcastFeedClient podcastClient,
    ILogger<ValidatePodcastNameCommandHandler> logger
) : IRequestHandler<ValidatePodcastNameCommand, bool>
{
    public async Task<bool> Handle(ValidatePodcastNameCommand request, CancellationToken cancellationToken)
    {
        var podcastName = request.Name;

        var exists = await podcastReadStore.Exists(podcastName);

        if (exists)
        {
            logger.LogWarning("Podcast ({PodcastName}) already exists in system", podcastName);

            return false;
        }

        var podcast = await podcastClient.GetPodcast(podcastName);

        if (podcast == null)
        {
            logger.LogWarning("Podcast ({PodcastName}) does not exist in DRs system", podcastName);

            return false;
        }

        logger.LogInformation("Podcast ({PodcastName}) is valid", podcastName);

        return true;
    }
}