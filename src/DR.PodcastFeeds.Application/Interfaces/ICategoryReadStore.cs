using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Application.Interfaces;

public interface ICategoryReadStore
{
    Task<IEnumerable<Category>> GetCategories();
}