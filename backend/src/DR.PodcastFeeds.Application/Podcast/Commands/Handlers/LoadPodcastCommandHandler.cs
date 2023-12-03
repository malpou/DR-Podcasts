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
        var podcastName = request.Podcast.Name;
        var reload = request.Reload;

        if (reload)
        {
            logger.LogInformation("Reloading podcast ({PodcastName}) from DRs system", podcastName);

            var newestPodcast = await podcastClient.GetPodcast(podcastName);

            if (newestPodcast == null)
            {
                logger.LogCritical("Podcast ({PodcastName}) does not exist in DRs system", podcastName);

                return;
            }

            var changed = await podcastWriteStore.Upsert(newestPodcast);

            if (changed)
            {
                logger.LogInformation("Podcast ({PodcastName}) loaded/reloaded to the system", podcastName);

                return;
            }

            logger.LogInformation("Podcast ({PodcastName}) is already up to date", podcastName);
        }
        else
        {
            logger.LogInformation("Initial load of podcast ({PodcastName}) from DRs system", podcastName);

            await podcastWriteStore.Upsert(request.Podcast);
        }
    }
}