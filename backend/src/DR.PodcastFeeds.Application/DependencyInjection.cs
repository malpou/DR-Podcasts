using DR.PodcastFeeds.Application.Podcast.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace DR.PodcastFeeds.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        services.AddScoped<PodcastUpdateScheduler>();
    }
}