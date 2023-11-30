using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class CategoryReadStore(IOptions<MongoDbSettings> settings)
    : MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName),
        ICategoryReadStore
{
    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await Collection.Aggregate()
            .Group(podcast => podcast.Category, group => new Category(
                group.Key,
                group.First().CategorySlug
            ))
            .SortBy(category => category.Name)
            .ToListAsync();

        return categories;
    }
}