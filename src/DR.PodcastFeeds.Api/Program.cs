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
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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