using DatabaseRepository.Constants;
using DatabaseRepository.Helper;
using DatabaseRepository.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public static string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public static bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out Guid guid) && guid != Guid.Empty;
        }

        public static void SendEmailNotification(EmailNotificationConfig emailNotificationConfig)
        {
            var smtpClient = new SmtpClient(emailNotificationConfig.SmtpHost, emailNotificationConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(emailNotificationConfig.SmtpUser, emailNotificationConfig.SmtpPassword),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(emailNotificationConfig.SmtpUser),
                Subject = emailNotificationConfig.Subject,
                Body = emailNotificationConfig.Body,
                IsBodyHtml = emailNotificationConfig.IsHtml,
            };

            if (emailNotificationConfig.To != null && emailNotificationConfig.To.ToString().Split(';').Count() > 0)
            {
                foreach (var toAddress in emailNotificationConfig.To.Split(';'))
                {
                    mail.To.Add(toAddress);
                }
            }

            if (emailNotificationConfig.Cc != null && emailNotificationConfig.Cc.ToString().Split(';').Count() > 0)
            {
                foreach (var toAddress in emailNotificationConfig.Cc.Split(';'))
                {
                    mail.CC.Add(toAddress);
                }
            }

            if (emailNotificationConfig.Bcc != null && emailNotificationConfig.Bcc.ToString().Split(';').Count() > 0)
            {
                foreach (var toAddress in emailNotificationConfig.Bcc.Split(';'))
                {
                    mail.Bcc.Add(toAddress);
                }
            }

            smtpClient.Send(mail);
        }
    }
}
