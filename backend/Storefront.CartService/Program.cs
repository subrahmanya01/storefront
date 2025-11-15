using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Storefront.ProductService;
using Storefront.CartService.Middlewares;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Storefront.CartService.Infrastructure;
using Storefront.ProductService.Protos;

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
builder.Services.AddGrpcClient<ProductRpc.ProductRpcClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:ProductServiceUrl"]?? throw new Exception("Product service url not found"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
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
app.RegisterGrpcServices();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
