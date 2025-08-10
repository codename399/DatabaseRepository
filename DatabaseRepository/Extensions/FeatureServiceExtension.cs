using Microsoft.Extensions.DependencyInjection;

namespace DatabaseRepository.Extensions
{
    public static class FeatureServiceExtension
    {
        public static void AddAutoMapperServiceDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
