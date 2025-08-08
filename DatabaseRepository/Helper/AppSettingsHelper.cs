using Microsoft.Extensions.Configuration;
using System.Reflection;

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
            T t = Activator.CreateInstance<T>();

            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize() first.");
            }

            foreach (PropertyInfo prop in t.GetType().GetProperties())
            {
                prop.SetValue(t, _configuration[$"{sectionName}:{prop.Name}"]);
            }

            return t ?? throw new ArgumentNullException($"Configuration '{sectionName}' not found.");
        }
    }
}
