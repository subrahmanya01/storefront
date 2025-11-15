using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Storefront.ProductService;
using Storefront.ProductService.Protos;
using Storefront.ProductService.Services.GrpcServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


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
builder.Services.AddGrpcClient<OrderRpc.OrderRpcClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:OrderServiceUrl"] ?? throw new Exception("Product service url not found"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServiceExtensions();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
