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
  /*  c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Input bearer token to access this API",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
            },
            new[] {"DemoSwaggerDifferentAuthScheme"}
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>(); */
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