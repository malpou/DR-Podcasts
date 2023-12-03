using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Api.Extensions;

public static class CategoryExtensions
{
    public static List<CategoryResponse> ToResponses(this IEnumerable<Category> categories)
    {
        return categories.Select(c => c.ToResponse()).ToList();
    }

    private static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse(category.Name, category.Slug);
    }
}