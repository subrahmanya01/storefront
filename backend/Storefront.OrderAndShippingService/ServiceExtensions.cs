using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Storefront.OrderAndShippingService.Infrastructure.Repository;
using Storefront.OrderAndShippingService.Services;
using Storefront.OrderAndShippingService.Services.GrpcClient;
using Storefront.OrderAndShippingService.Services.GrpcServices;

namespace Storefront.OrderAndShippingService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddFluentValidationAutoValidation();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ITaxRateService, TaxRateService>();
            services.AddScoped<IShippingChargeService, ShippingChargeService>();
            services.AddScoped<IOrderPriceService, OrderPriceService>();

            return services;
        }

        public static IServiceCollection RegisterGrpcClients(this IServiceCollection services)
        {
            services.AddScoped<ICartRpcService, CartRpcService>();
            services.AddScoped<IProductRpcService, ProductRpcService>();
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
