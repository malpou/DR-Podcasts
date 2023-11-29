using Microsoft.AspNetCore.Http.HttpResults;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class CategoriesEndpoint
{
    public static async Task<IResult> Handle()
    {
        return Results.Ok();
    }
}