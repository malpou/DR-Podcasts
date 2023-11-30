using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Categories.Queries.Handlers;

public class GetCategoriesQueryHandler(
    ICategoryReadStore categoryReadStore,
    ILogger<GetCategoriesQueryHandler> logger)  : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = (await categoryReadStore.GetCategories()).ToList();

        logger.LogInformation("Found {CategoryCount} categories", categories.Count);

        return categories;
    }
}