using DR.PodcastFeeds.Application.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class UpdatePodcastsCommandHandler(
        IPodcastReadStore podcastReadStore,
        IBackgroundJobClient backgroundJob,
        ILogger<UpdatePodcastsCommandHandler> logger)
    : IRequestHandler<UpdatePodcastsCommand>
{
    public async Task Handle(UpdatePodcastsCommand request, CancellationToken cancellationToken)
    {
        var podcasts = await podcastReadStore.GetAll();

        foreach (var podcast in podcasts)
        {
            var loadPodcastCommand = new LoadPodcastCommand(podcast.Name);
            var jobId = backgroundJob.Enqueue<ISender>(x => x.Send(loadPodcastCommand, cancellationToken));

            logger.LogInformation("Podcast ({PodcastName}) added to queue with job id ({JobId})", podcast.Name, jobId);
        }
    }
}