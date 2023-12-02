using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Application.Podcast.Commands;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Jobs;

public class PodcastUpdateScheduler(
    IPodcastReadStore podcastReadStore,
    IBackgroundJobClient backgroundJob,
    IRecurringJobManager recurringJob,
    ILogger<PodcastUpdateScheduler> logger)
{
    public void SchedulePodcastsUpdates()
    {
        recurringJob.AddOrUpdate("podcasts-update-job",
            () => ReloadPodcasts(),
            Cron.Minutely);
    }

    // Have to be public for Hangfire to be able to call it
    public void ReloadPodcasts()
    {
        var podcasts = podcastReadStore.GetAll().Result;

        foreach (var podcast in podcasts)
        {
            var loadPodcastCommand = new LoadPodcastCommand(podcast);
            var jobId = backgroundJob.Enqueue<ISender>(x => x.Send(loadPodcastCommand, default));

            logger.LogInformation("Podcast ({PodcastName}) added to queue with job id ({JobId})", podcast.Name,
                jobId);
        }
    }
}