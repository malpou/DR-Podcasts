using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Categories.Queries;
using MediatR;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class CategoriesEndpoint
{
    public static async Task<IResult> Handle(ISender sender)
    {
        var categories = await sender.Send(new GetCategoriesQuery());

        var categoriesList = categories.ToList();
        
        return categoriesList.Any() 
            ? Results.Ok(categoriesList.ToResponses()) 
            : Results.NoContent();

    }
}