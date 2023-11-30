using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class LoadPodcastCommandHandler(
        IPodcastFeedClient podcastClient,
        IPodcastWriteStore podcastWriteStore,
        ILogger<LoadPodcastCommandHandler> logger)
    : IRequestHandler<LoadPodcastCommand>
{
    public async Task Handle(LoadPodcastCommand request, CancellationToken cancellationToken)
    {
        var podcastName = request.Name;

        var podcast = await podcastClient.GetPodcast(podcastName);

        if (podcast == null)
        {
            logger.LogCritical("Podcast ({PodcastName}) does not exist in DRs system", podcastName);

            return;
        }

        var changed = await podcastWriteStore.Upsert(podcast);

        if (changed)
        {
            logger.LogInformation("Podcast ({PodcastName}) loaded/reloaded to the system", podcastName);

            return;
        }

        logger.LogInformation("Podcast ({PodcastName}) is already up to date", podcastName);
    }
}