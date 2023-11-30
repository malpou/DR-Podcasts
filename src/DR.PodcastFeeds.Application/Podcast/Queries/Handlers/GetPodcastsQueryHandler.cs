using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Queries.Handlers;

public class GetPodcastsQueryHandler
    (IPodcastReadStore podcastReadStore, ILogger<GetPodcastsQueryHandler> logger) : IRequestHandler<GetPodcastsQuery,
        IEnumerable<Domain.Podcast>>
{
    public async Task<IEnumerable<Domain.Podcast>> Handle(GetPodcastsQuery request, CancellationToken cancellationToken)
    {
        var category = request.category;

        if (category is null)
            logger.LogInformation("Getting all podcasts");
        else
            logger.LogInformation("Getting podcasts in category {Category}", category);

        return await podcastReadStore.GetAll(category);
    }
}