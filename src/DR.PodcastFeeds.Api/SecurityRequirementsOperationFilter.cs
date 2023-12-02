using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DR.PodcastFeeds.Api;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiredScopes = context.MethodInfo.DeclaringType
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Select(attr => attr.AuthenticationSchemes)
            .Distinct();

        var requireAuth = false;
        var id="";

        if (requiredScopes.Contains("Bearer"))
        {
            requireAuth = true;
            id = "bearerAuth";
        }

        if (!requireAuth || string.IsNullOrEmpty(id)) return;

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
                    },
                    new[] { "DemoSwaggerDifferentAuthScheme" }
                }
            }
        };
    }
}