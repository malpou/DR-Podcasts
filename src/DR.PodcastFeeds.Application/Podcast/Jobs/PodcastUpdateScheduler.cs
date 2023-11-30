using DR.PodcastFeeds.Application.Podcast.Commands;
using Hangfire;
using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Jobs;

public class PodcastUpdateScheduler(
    ISender sender,
    IRecurringJobManager recurringJob)
{
    public void SchedulePodcastUpdates()
    {
        recurringJob.AddOrUpdate("podcast-update-job",
            () => sender.Send(new UpdatePodcastsCommand(), default),
            "0,15,30,45 * * * *");
    }
}