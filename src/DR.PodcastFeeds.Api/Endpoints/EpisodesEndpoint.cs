using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Episodes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class EpisodesEndpoint
{
    public static async Task<IResult> Handle(
        [FromQuery] int? page,
        [FromQuery] int? size,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] int? last,
        ISender sender
    )
    {
        if ((page.HasValue && !size.HasValue) || (!page.HasValue && size.HasValue))
            return Results.BadRequest("Both 'page' and 'size' parameters must be provided together.");

        if ((from.HasValue && !to.HasValue) || (!from.HasValue && to.HasValue))
            return Results.BadRequest("Both 'from' and 'to' parameters must be provided together.");

        if (from.HasValue && to.HasValue)
        {
            var fromDate = from.Value;
            var toDate = to.Value;
            if (fromDate > toDate)
                return Results.BadRequest("'from' must be before 'to'.");
        }

        var episodes = await sender.Send(new GetEpisodesQuery(null, page, size, from, to, last));

        var episodesList = episodes.ToList();

        return !episodesList.Any()
            ? Results.NotFound()
            : Results.Ok(episodesList.Select(e => e.ToResponse(e.Podcast?.Title, e.Podcast?.ImageUrl)));
    }
}