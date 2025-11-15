using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Storefront.CartService.Infrastructure.Repository;
using Storefront.CartService.Services;
using Storefront.CartService.Services.GrpcClients;
using Storefront.CartService.Services.GrpcServer;
namespace Storefront.ProductService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddFluentValidationAutoValidation();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<ICartService, Storefront.CartService.Services.CartService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            return services;
        }
        public static WebApplication RegisterGrpcServices(this WebApplication app)
        {
            app.MapGrpcService<CartRpcService>();
            return app;
        }
        public static IServiceCollection RegisterGrpcClients(this IServiceCollection services)
        {
            services.AddSingleton<IProductGrpcClient, ProductGrpcClient>();
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
