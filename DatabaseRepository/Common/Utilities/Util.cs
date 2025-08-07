using DatabaseRepository.Helper;
using DatabaseRepository.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DatabaseRepository.Common.Utilities
{
    public static class Util
    {
        public static string GenerateJwtToken(HashSet<Claim> claims, string configSectionName = "AuthenticationConfig")
        {
            AuthenticationConfig authenticationConfig = JsonSerializer.Deserialize<AuthenticationConfig>(AppSettingsHelper.GetConfiguration(configSectionName)) ?? new AuthenticationConfig();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfig.SecretKey ?? string.Empty));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: authenticationConfig.Issuer,
                audience: authenticationConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(authenticationConfig.AccessTokenExpirationHours ?? 1),
                signingCredentials: creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
