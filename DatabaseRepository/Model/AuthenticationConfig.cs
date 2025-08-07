using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DatabaseRepository.Model
{
    public class AuthenticationConfig
    {
        public string? AuthenticateScheme { get; set; }
        public string? ChallengeScheme { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateLifeIssuerSigningKey { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
        public double? AccessTokenExpirationHours { get; set; }

        public AuthenticationConfig()
        {
            AuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            ChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
