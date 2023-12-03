using Microsoft.OpenApi.Models;

namespace DR.PodcastFeeds.Api;

public static class SwaggerExtensions
{
    public static RouteHandlerBuilder WithBearerToken(this RouteHandlerBuilder builder)
    {
        builder.WithOpenApi(operation =>
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }
                    ] = new string[] { }
                }
            };

            return operation;
        });

        return builder;
    }
}