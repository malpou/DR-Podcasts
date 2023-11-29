using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class EpisodesEndpoint
{
    public static async Task<IResult> Handle(
        [FromRoute] string name,
        [FromQuery] int? page,
        [FromQuery] int? size,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] int? last
        )
    {
        if ((page.HasValue && !size.HasValue) || (!page.HasValue && size.HasValue))
        {
            return Results.BadRequest("Both 'page' and 'size' parameters must be provided together.");
        }
        
        if ((from.HasValue && !to.HasValue) || (!from.HasValue && to.HasValue))
        {
            return Results.BadRequest("Both 'from' and 'to' parameters must be provided together.");
        }
        
        return Results.Ok();
    }
}