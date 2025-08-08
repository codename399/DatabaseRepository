using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DatabaseRepository.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerService(this IServiceCollection services, bool addAuthentication = true)
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
                    // Add JWT Authentication
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter your valid JWT token.\n\nExample: `abc123`"
                    });

                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            });
        }
    }
}
