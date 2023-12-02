using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Application.Podcast.Jobs;
using DR.PodcastFeeds.Contracts;

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
            .Produces<List<CategoryResponse>>();

        // Podcasts Endpoints
        app.MapGet(PodcastsPath, PodcastsEndpoint.Handle)
            .WithTags("Podcasts")
            .WithDescription("Get all podcasts")
            .Produces<List<PodcastResponse>>();
        app.MapGet($"{CategoriesPath}/{{category}}/{PodcastsPath}", PodcastsByCategoryEndpoint.Handle)
            .WithTags("Podcasts")
            .WithDescription("Get all podcasts by category")
            .Produces<List<PodcastResponse>>();
        app.MapPost(PodcastsPath, AddPodcastEndpoint.Handle)
            .RequireAuthorization("Admin")
            .WithTags("Podcasts");
        app.MapDelete(PodcastsPath, DeletePodcastEndpoint.Handle)
            .RequireAuthorization("Admin")
            .WithTags("Podcasts");

        // Episodes Endpoints
        app.MapGet($"{PodcastsPath}/{{name}}/{EpisodesPath}", EpisodesByPodcastEndpoint.Handle)
            .WithTags("Episodes")
            .WithDescription("Get episodes for a podcast")
            .Produces<EpisodesResponse>();
        app.MapGet($"{EpisodesPath}", EpisodesEndpoint.Handle)
            .WithTags("Episodes")
            .WithDescription("Get all episodes")
            .Produces<List<EpisodeResponse>>();

        // Login Endpoints
        app.MapPost(LoginPath, LoginEndpoint.Handle)
            .WithTags("Admin")
            .Produces<LoginResponse>();
        
        // Register Endpoints
        if (app.Environment.IsDevelopment())
        {
            app.MapPost(RegisterPath, RegisterAdminEndpoint.Handle)
                .WithTags("Admin");
        }
    }
    
    public static void AddScheduler(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var podcastUpdateScheduler = scope.ServiceProvider.GetRequiredService<PodcastUpdateScheduler>();
        podcastUpdateScheduler.SchedulePodcastUpdates();
    }
}