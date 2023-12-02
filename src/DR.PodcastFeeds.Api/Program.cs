using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Application;
using DR.PodcastFeeds.Application.Podcast.Jobs;
using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .WithHeaders("Authorization", "Content-Type"));

    options.AddPolicy("ProdPolicy", x => x
        .WithOrigins("https://podcasts.malpou.dev", "https://dr-podcasts.vercel.app/")
        .WithMethods("GET", "POST", "DELETE")
        .WithHeaders("Authorization", "Content-Type"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseHttpsRedirection();

app.UseCors(app.Environment.IsDevelopment() ? "DevPolicy" : "ProdPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
const string categoriesPath = "categories";
const string podcastsPath = "podcasts";
const string episodesPath = "episodes";
const string registerPath = "register";
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
    .RequireAuthorization("Admin")
    .WithTags("Podcasts");
app.MapDelete(podcastsPath, DeletePodcastEndpoint.Handle)
    .RequireAuthorization("Admin")
    .WithTags("Podcasts");

// Episodes Endpoints
app.MapGet($"{podcastsPath}/{{name}}/{episodesPath}", EpisodesByPodcastEndpoint.Handle)
    .WithTags("Episodes")
    .WithDescription("Get episodes for a podcast")
    .Produces<EpisodesResponse>();
app.MapGet($"{episodesPath}", EpisodesEndpoint.Handle)
    .WithTags("Episodes")
    .WithDescription("Get all episodes")
    .Produces<List<EpisodeResponse>>();

// Login Endpoints
app.MapPost(loginPath, LoginEndpoint.Handle)
    .WithTags("Admin")
    .Produces<LoginResponse>();
if (app.Environment.IsDevelopment())
    app.MapPost(registerPath, RegisterAdminEndpoints.Handle)
        .WithTags("Admin");

// Scheduler
using var scope = app.Services.CreateScope();
var podcastUpdateScheduler = scope.ServiceProvider.GetRequiredService<PodcastUpdateScheduler>();
podcastUpdateScheduler.SchedulePodcastUpdates();

app.Run();