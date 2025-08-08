using Microsoft.Extensions.Configuration;

namespace DatabaseRepository.Helper
{
    public static class AppSettingsHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static T GetConfiguration<T>(string sectionName)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize() first.");
            }

            return (T)_configuration.GetSection(sectionName) ?? throw new ArgumentNullException($"Configuration '{sectionName}' not found.");
        }
    }
}
