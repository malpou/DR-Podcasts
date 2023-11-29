using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public class DeletePodcastEndpoint
{
    public static async Task<IResult> Handle([FromQuery] string name)
    {
        return Results.Ok();
    }
}