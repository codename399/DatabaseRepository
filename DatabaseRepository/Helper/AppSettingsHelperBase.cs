using Microsoft.Extensions.Configuration;

namespace DatabaseRepository.Helper
{
    public class AppSettingsHelperBase
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfiguration(string sectionName)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize() first.");
            }

            return _configuration[sectionName] ?? throw new ArgumentNullException($"Configuration '{sectionName}' not found.");
        }
    }
}
