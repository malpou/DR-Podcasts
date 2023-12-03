using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Podcast.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class PodcastsByCategoryEndpoint
{
    public static async Task<IResult> Handle([FromRoute] string categorySlug, ISender sender)
    {
        var podcasts = await sender.Send(new GetPodcastsQuery(categorySlug));

        var podcastsList = podcasts.ToList();

        return podcastsList.Any() ? Results.Ok(podcastsList.ToResponses()) : Results.NotFound();
    }
}