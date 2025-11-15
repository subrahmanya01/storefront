using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Storefront.OrderAndShippingService;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Protos;
using Storefront.OrderAndShippingService.Services;
using Storefront.OrderAndShippingService.Services.GrpcServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(optios =>
    {
        optios.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"),
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SecretToken")!)),
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddGrpc();
builder.Services.AddGrpcClient <CartRpc.CartRpcClient> (options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:CartServiceUrl"] ?? throw new Exception("Cart service url not found"));
});

builder.Services.AddGrpcClient<ProductRpc.ProductRpcClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:ProductServiceUrl"] ?? throw new Exception("Product service url not found"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServiceExtensions();
builder.Services.RegisterGrpcClients();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGrpcService<OrderRpcService>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
