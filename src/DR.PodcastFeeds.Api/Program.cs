using DR.PodcastFeeds.Api;
using DR.PodcastFeeds.Application;
using DR.PodcastFeeds.Infrastructure;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

var app = builder.Build();
app.UseHttpsRedirection();

app.UseCors(app.Environment.IsDevelopment() ? "DevPolicy" : "ProdPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.DefineEndpoints();
app.AddScheduler();

app.Run();