using DatabaseRepository.Constants;
using DatabaseRepository.Helper;
using DatabaseRepository.Model;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DatabaseRepository.Common.Utilities
{
    public static class Util
    {
        public static string GenerateJwtToken(HashSet<Claim> claims, string configSectionName = BaseConstant.AuthenticationConfig)
        {
            AuthenticationConfig authenticationConfig = AppSettingsHelper.GetConfiguration<AuthenticationConfig>(configSectionName) ?? new AuthenticationConfig();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfig.SecretKey ?? string.Empty));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: authenticationConfig.Issuer,
                audience: authenticationConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(authenticationConfig.AccessTokenExpirationHours ?? "1.0")),
                signingCredentials: creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
