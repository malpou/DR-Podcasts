using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Application;
using DR.PodcastFeeds.Application.Podcast.Jobs;
using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints
const string categoriesPath = "categories";
const string podcastsPath = "podcasts";
const string episodesPath = "episodes";
const string loginPath = "login";

// Categories Endpoints
app.MapGet(categoriesPath, CategoriesEndpoint.Handle)
    .WithTags("Categories")
    .WithDescription("Get all categories")
    .Produces<List<CategoryResponse>>();

// Podcasts Endpoints
app.MapGet(podcastsPath, PodcastsEndpoint.Handle)
    .WithTags("Podcasts")
    .WithDescription("Get all podcasts")
    .Produces<List<PodcastResponse>>();
app.MapGet($"{categoriesPath}/{{category}}/{podcastsPath}", PodcastsByCategoryEndpoint.Handle)
    .WithTags("Podcasts")
    .WithDescription("Get all podcasts by category")
    .Produces<List<PodcastResponse>>();
app.MapPost(podcastsPath, AddPodcastEndpoint.Handle)
    //.RequireAuthorization("Admin")
    .WithTags("Podcasts");
app.MapDelete(podcastsPath, DeletePodcastEndpoint.Handle)
    //.RequireAuthorization("Admin")
    .WithTags("Podcasts");

// Episodes Endpoints
app.MapGet($"{podcastsPath}/{{name}}/{episodesPath}", EpisodesByPodcastEndpoint.Handle)
    .WithTags("Episodes")
    .WithDescription("Get episodes for a podcast")
    .Produces<EpisodesResponse>();

// Login Endpoints
app.MapPost(loginPath, LoginEndpoint.Handle)
    .WithTags("Login");

using var scope = app.Services.CreateScope();
var podcastUpdateScheduler = scope.ServiceProvider.GetRequiredService<PodcastUpdateScheduler>();
podcastUpdateScheduler.SchedulePodcastUpdates();

app.Run();