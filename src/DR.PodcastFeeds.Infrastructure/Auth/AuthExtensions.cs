using System.Security.Claims;
using System.Text;
using DR.PodcastFeeds.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DR.PodcastFeeds.Infrastructure.Auth;

public static class AuthExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        var keyVaultService = services.BuildServiceProvider().GetRequiredService<ISecretsService>();
        var jwtSecret = keyVaultService.GetJwtSecret().GetAwaiter().GetResult();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "DR.PodcastFeeds",
                    ValidAudience = "DR.PodcastFeeds",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
    }

    public static void AddAdminAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var userClaims = context.User.Claims;
                    var user = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                    return user?.Value == "Admin";
                });
            });
    }
}