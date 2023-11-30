﻿using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record DeletePodcastCommand(string Name) : IRequest<(bool, string)>;