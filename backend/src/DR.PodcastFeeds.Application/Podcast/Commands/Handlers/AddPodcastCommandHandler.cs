using DR.PodcastFeeds.Application.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class AddPodcastCommandHandler(
    IPodcastReadStore podcastReadStore,
    IPodcastFeedClient podcastClient,
    IBackgroundJobClient backgroundJob,
    ILogger<AddPodcastCommandHandler> logger
) : IRequestHandler<AddPodcastCommand, (bool, string)>
{
    public async Task<(bool, string)> Handle(
        AddPodcastCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var podcastName = request.Name;

        var exists = await podcastReadStore.PodcastsExists(podcastName);

        if (exists)
        {
            logger.LogWarning("Podcast ({PodcastName}) already exists in system", podcastName);

            return (false, "Podcast already exists in system");
        }

        var podcast = await podcastClient.GetPodcast(podcastName);

        if (podcast == null)
        {
            logger.LogWarning("Podcast ({PodcastName}) does not exist in DRs system", podcastName);

            return (false, "Podcast does not exist in DRs system");
        }

        backgroundJob.Enqueue<ISender>(x => x.Send(new LoadPodcastCommand(podcast, false), cancellationToken));

        logger.LogInformation("Podcast ({Podcast}) added to system", podcastName);

        return (true, "Podcast added to system");
    }
}