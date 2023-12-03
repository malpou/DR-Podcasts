using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Episodes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class EpisodesByPodcastEndpoint
{
    public static async Task<IResult> Handle(
        [FromRoute] string podcastSlug,
        [FromQuery] int? page,
        [FromQuery] int? size,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] int? last,
        ISender sender
    )
    {
        var validationResult = EndpointValidationHelper.ValidateCommonParameters(page, size, from, to, last);
        if (validationResult is not null)
            return validationResult;

        var episodes = await sender.Send(new GetEpisodesQuery(podcastSlug, page, size, from, to, last));

        if (episodes is null)
            return Results.NotFound();

        var episodesList = episodes.ToList();
        return episodesList.Any()
            ? Results.Ok(episodesList.ToResponse())
            : Results.NoContent();
    }
}