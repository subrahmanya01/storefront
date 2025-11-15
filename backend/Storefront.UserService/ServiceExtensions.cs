using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Storefront.UserService.Infrastructure.Repository;
using Storefront.UserService.Services;

namespace Storefront.UserService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddFluentValidationAutoValidation();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IUserService, Services.UserService>();
            services.AddScoped<IWhishListService, WhishListService>();
            services.AddScoped<IWhishListRepository, WhishListRepository >();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT Bearer token below:\n\nExample: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });
            return services;
        }
    }
}
