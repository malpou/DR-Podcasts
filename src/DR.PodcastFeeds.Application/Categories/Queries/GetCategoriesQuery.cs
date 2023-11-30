using DR.PodcastFeeds.Domain;
using MediatR;

namespace DR.PodcastFeeds.Application.Categories.Queries;

public record GetCategoriesQuery() : IRequest<IEnumerable<Category>>;