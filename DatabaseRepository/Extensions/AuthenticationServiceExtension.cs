using DatabaseRepository.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameStash.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services, AuthenticationConfig authenticationConfig)
        {
            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = authenticationConfig.ValidateAudience,
                    ValidateIssuer = authenticationConfig.ValidateIssuer,
                    ValidateLifetime = authenticationConfig.ValidateLifetime,
                    ValidateIssuerSigningKey = authenticationConfig.ValidateLifeIssuerSigningKey,
                    ValidIssuer = authenticationConfig.Issuer,
                    ValidAudience = authenticationConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authenticationConfig.SecretKey ?? string.Empty)
                        )
                };
            });
        }
    }
}
