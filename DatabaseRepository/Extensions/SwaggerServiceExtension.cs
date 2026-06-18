using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace DatabaseRepository.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerService(
            this IServiceCollection services,
            bool addAuthentication = true)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CodeName399 API",
                    Version = "v1",
                    Description = "API for CodeName399 applications"
                });

                if (addAuthentication)
                {
                    option.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "Enter your JWT token"
                        });

                    option.AddSecurityRequirement(document =>
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                                new List<string>()
                        });
                }
            });
        }
    }
}