using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Write;

public class PodcastWriteStore(IOptions<MongoDbSettings> settings)
    : MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName), IPodcastWriteStore
{
    public async Task<bool> Upsert(Podcast podcast)
    {
        var record = podcast.ToRecord();
        var existingRecord = await Collection.Find(p => p.Name == podcast.Name).FirstOrDefaultAsync();

        if (existingRecord != null && IsRecordChanged(existingRecord, record))
        {
            record.Updated = DateTime.UtcNow;
            var filter = Builders<PodcastRecord>.Filter.Eq(p => p.Name, podcast.Name);
            var options = new ReplaceOptions {IsUpsert = true};
            await Collection.ReplaceOneAsync(filter, record, options);

            return true;
        }

        if (existingRecord != null) return false;

        await Collection.InsertOneAsync(record);

        return true;
    }

    public async Task<bool> Remove(string name)
    {
        var filter = Builders<PodcastRecord>.Filter.Eq(podcast => podcast.Name, name);

        return await Collection.DeleteOneAsync(filter).ContinueWith(task => task.Result.DeletedCount > 0);
    }

    private static bool IsRecordChanged(PodcastRecord existingRecord, PodcastRecord newRecord)
    {
        return newRecord.Episodes != null && existingRecord.Episodes != null &&
               (existingRecord.Title != newRecord.Title ||
                existingRecord.Description != newRecord.Description ||
                existingRecord.ImageUrl != newRecord.ImageUrl ||
                existingRecord.Category != newRecord.Category ||
                existingRecord.CategorySlug != newRecord.CategorySlug ||
                existingRecord.Link != newRecord.Link ||
                existingRecord.Episodes.Length != newRecord.Episodes.Length);
    }
}