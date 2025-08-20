using DatabaseRepository.Constants;
using DatabaseRepository.Helper;
using DatabaseRepository.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

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

        public static bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out Guid guid) && guid != Guid.Empty;
        }
    }
}
