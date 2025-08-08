using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DatabaseRepository.Model
{
    public class AuthenticationConfig
    {
        public string? AuthenticateScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;
        public string? ChallengeScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;
        public bool? ValidateAudience { get; set; } = true;
        public bool? ValidateIssuer { get; set; } = true;
        public bool? ValidateLifetime { get; set; } = true;
        public bool? ValidateLifeIssuerSigningKey { get; set; } = true;
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
        public string? AccessTokenExpirationHours { get; set; }
    }
}
