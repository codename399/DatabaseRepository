using DatabaseRepository.Constants;
using DatabaseRepository.Helper;
using DatabaseRepository.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameStash.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services, string configSectionName = BaseConstant.AuthenticationConfig)
        {
            AuthenticationConfig authenticationConfig = AppSettingsHelper.GetConfiguration<AuthenticationConfig>(configSectionName) ?? new AuthenticationConfig();

            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = authenticationConfig.AuthenticateScheme ?? JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = authenticationConfig.ChallengeScheme ?? JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = authenticationConfig.ValidateAudience ?? true,
                    ValidateIssuer = authenticationConfig.ValidateIssuer ?? true,
                    ValidateLifetime = authenticationConfig.ValidateLifetime ?? true,
                    ValidateIssuerSigningKey = authenticationConfig.ValidateLifeIssuerSigningKey ?? true,
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
