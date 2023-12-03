using MediatR;

namespace DR.PodcastFeeds.Application.Admin.Commands;

public record AddAdminCredentialsCommand(string Username, string Password) : IRequest<bool>;