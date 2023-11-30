using DR.PodcastFeeds.Domain;
using MediatR;

namespace DR.PodcastFeeds.Application.Episodes.Queries;

public record GetEpisodesQuery(
    string? PodcastName,
    int? PageNumber,
    int? PageSize,
    DateOnly? FromDate,
    DateOnly? ToDate,
    int? Last
     ): IRequest<IEnumerable<Episode>>;