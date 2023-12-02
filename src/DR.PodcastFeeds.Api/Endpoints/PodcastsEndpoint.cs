using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Podcast.Queries;
using MediatR;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class PodcastsEndpoint
{
    public static async Task<IResult> Handle(ISender sender)
    {
        var podcasts = await sender.Send(new GetPodcastsQuery());

        var podcastsList = podcasts.ToList();
        
        return podcastsList.Any() 
            ? Results.Ok(podcastsList.ToResponses()) 
            : Results.NoContent();
    }
}