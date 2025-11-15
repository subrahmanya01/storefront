using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Storefront.ProductService.Infrastructure.Repository;
using Storefront.ProductService.Services;
using Storefront.ProductService.Services.GrpcClient;
using Storefront.ProductService.Services.GrpcServices;
namespace Storefront.ProductService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddFluentValidationAutoValidation();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductRatingRepository, ProductRatingRepository>();

            services.AddScoped<IProductService, Services.ProductService>();
            services.AddScoped<IProductRatingService, ProductRatingService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IProductInfoService, ProductInfoService>();
            services.AddScoped<IFlashSaleRepository, FlashSaleRepository>();

            services.AddScoped<IBlobService, BlobService>();
            
            services.AddScoped<IOrderRpcService, OrderRpcService>();
            return services;
        }

        public static WebApplication RegisterGrpcServices(this WebApplication app)
        {
            app.MapGrpcService<ProductRpcService>();
            return app;
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
