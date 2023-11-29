using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class PodcastsByCategoryEndpoint
{
    public static async Task<IResult> Handle([FromRoute] string category)
    {
        return Results.Ok();
    }
}