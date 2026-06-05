using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BuildingBlocks.Authorization;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"], // Issuer
                ValidAudience = configuration["JwtSettings:Audience"], // Audience
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
            };

            // Hook into the OnMessageReceived event to modify the token
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].ToString();

                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtExtensions");
                    if(string.IsNullOrEmpty(token))
                    {
                        logger.LogError("No token found");
                    }
                    else
                    {
                        //logger.LogError("Token found: {token}", token);
                    }

                    // Check if the token has the 'Bearer ' prefix and remove it
                    //if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
                    //{
                    //    // Remove the 'Bearer ' prefix
                    //    context.Request.Headers["Authorization"] = token.Substring("Bearer ".Length);
                    //}

                    return Task.CompletedTask;
                }
            };
        });
    }
}
