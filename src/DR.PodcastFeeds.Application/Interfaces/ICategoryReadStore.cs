namespace DR.PodcastFeeds.Application.Interfaces;

public interface ICategoryReadStore
{
    Task<IEnumerable<Domain.Category>> GetCategories();
}