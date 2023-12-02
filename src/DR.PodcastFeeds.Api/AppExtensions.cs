using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Application.Podcast.Jobs;
using DR.PodcastFeeds.Contracts;
using Microsoft.OpenApi.Models;

namespace DR.PodcastFeeds.Api;

public static class AppExtensions
{
    private const string CategoriesPath = "categories";
    private const string PodcastsPath = "podcasts";
    private const string EpisodesPath = "episodes";
    private const string RegisterPath = "register";
    private const string LoginPath = "login";

    public static void DefineEndpoints(this WebApplication app)
    {
        // Categories Endpoints
        app.MapGet(CategoriesPath, CategoriesEndpoint.Handle)
            .WithTags("Categories")
            .WithDescription("Get all categories")
            .Produces<List<CategoryResponse>>()
            .Produces(204);

        // Podcasts Endpoints
        app.MapGet(PodcastsPath, PodcastsEndpoint.Handle)
            .WithTags("Podcasts")
            .WithDescription("Get all podcasts")
            .Produces<List<PodcastResponse>>()
            .Produces(204);

        app.MapGet($"{CategoriesPath}/{{category}}/{PodcastsPath}", PodcastsByCategoryEndpoint.Handle)
            .WithTags("Podcasts")
            .WithDescription("Get all podcasts by category")
            .Produces<List<PodcastResponse>>()
            .Produces(204);

        app.MapPost(PodcastsPath, AddPodcastEndpoint.Handle)
            .RequireAuthorization("Admin")
            .WithBearerToken()
            .WithTags("Podcasts")
            .WithDescription("Add a podcast")
            .Produces(201)
            .Produces(400)
            .Produces(401);

        app.MapDelete(PodcastsPath, DeletePodcastEndpoint.Handle)
            .RequireAuthorization("Admin")
            .WithBearerToken()
            .WithTags("Podcasts")
            .WithDescription("Delete a podcast")
            .Produces(204)
            .Produces(404)
            .Produces(401);

        // Episodes Endpoints
        app.MapGet($"{PodcastsPath}/{{name}}/{EpisodesPath}", EpisodesByPodcastEndpoint.Handle)
            .WithTags("Episodes")
            .WithDescription("Get episodes for a podcast")
            .Produces<EpisodesResponse>()
            .Produces(204);

        app.MapGet($"{EpisodesPath}", EpisodesEndpoint.Handle)
            .WithTags("Episodes")
            .WithDescription("Get all episodes")
            .Produces<List<EpisodeResponse>>()
            .Produces(204);

        // Login Endpoints
        app.MapPost(LoginPath, LoginEndpoint.Handle)
            .WithTags("Admin")
            .WithDescription("Login as admin")
            .Produces<LoginResponse>()
            .Produces(401)
            .Produces(400);

        // Register Endpoints
        if (app.Environment.IsDevelopment())
        {
            app.MapPost(RegisterPath, RegisterAdminEndpoint.Handle)
                .WithTags("Admin")
                .WithDescription("Register as admin")
                .Produces(201)
                .Produces(400);
        }
    }

    public static void AddScheduler(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var podcastUpdateScheduler = scope.ServiceProvider.GetRequiredService<PodcastUpdateScheduler>();
        podcastUpdateScheduler.SchedulePodcastsUpdates();
    }
}