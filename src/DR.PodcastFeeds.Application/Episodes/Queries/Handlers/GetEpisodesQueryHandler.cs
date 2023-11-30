using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Episodes.Queries.Handlers;

public class GetEpisodesQueryHandler(
    IEpisodeReadStore episodeReadStore,
    IPodcastReadStore podcastReadStore,
    ILogger<GetEpisodesQueryHandler> logger) : IRequestHandler<GetEpisodesQuery, IEnumerable<Episode>>
{
    public async Task<IEnumerable<Episode>> Handle(GetEpisodesQuery request, CancellationToken cancellationToken)
    {
        var podcastName = request.PodcastName;
        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;
        var fromDate = request.FromDate;
        var toDate = request.ToDate;
        var last = request.Last;

        if (!string.IsNullOrWhiteSpace(podcastName))
        {
            var exists = await podcastReadStore.Exists(podcastName);
            
            if (!exists)
            {
                logger.LogInformation("Podcast {PodcastName} does not exist", podcastName);
                
                return Enumerable.Empty<Episode>();
            }
        }
        
        if (last.HasValue)
        {
            if (string.IsNullOrWhiteSpace(podcastName))
            {
                logger.LogInformation("Getting last {Last} episodes", last);
            }
            else
            {
                logger.LogInformation("Getting last {Last} episodes for podcast {PodcastName}", last, podcastName);
            }
            
            return await episodeReadStore.GetEpisodes(last.Value, podcastName);
        }
        
        if (fromDate.HasValue && toDate.HasValue)
        {
            if (string.IsNullOrWhiteSpace(podcastName))
            {
                logger.LogInformation("Getting episodes from {FromDate} to {ToDate}", fromDate, toDate);
            }
            else
            {
                logger.LogInformation("Getting episodes for podcast {PodcastName} from {FromDate} to {ToDate}", podcastName, fromDate, toDate);
            }
            
            return await episodeReadStore.GetEpisodes(fromDate.Value, toDate.Value, podcastName);
        }
        
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            if (string.IsNullOrWhiteSpace(podcastName))
            {
                logger.LogInformation("Getting episodes page {PageNumber} with size {PageSize}", pageNumber, pageSize);
            }
            else
            {
                logger.LogInformation("Getting episodes page {PageNumber} with size {PageSize} for podcast {PodcastName}", pageNumber, pageSize, podcastName);
            }
            
            return await episodeReadStore.GetEpisodes(pageNumber.Value, pageSize.Value, podcastName);
        }
        
        if (string.IsNullOrWhiteSpace(podcastName))
        {
            logger.LogInformation("Getting all episodes");
        }
        else
        {
            logger.LogInformation("Getting all episodes for podcast {PodcastName}", podcastName);
        }
        
        var episodes = await episodeReadStore.GetEpisodes(podcastName);
        
        return episodes;
    }
}