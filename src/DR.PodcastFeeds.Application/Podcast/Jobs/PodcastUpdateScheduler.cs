using DR.PodcastFeeds.Application.Interfaces;
using Hangfire;

namespace DR.PodcastFeeds.Application.Podcast.Jobs;

public class PodcastUpdateScheduler(IPodcastManager podcastManager)
{
    public void SchedulePodcastUpdates()
    {
        RecurringJob.AddOrUpdate("podcast-update-job", 
            () => podcastManager.UpdatePodcasts(), 
            "0,15,30,45 * * * *");
    }
}