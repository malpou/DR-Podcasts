using MediatR;

namespace DR.PodcastFeeds.Application.Admin.Commands;

public record LoginCommand(string Username, string Password) : IRequest<string?>;