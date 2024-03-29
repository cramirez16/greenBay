using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Src.Data;
using Src.helpers;
using Src.Services;
using Src.Services.IServices;
using Src.Repository;
using Src.Repository.IRepository;
using src.Services;
using src.Services.IServices;

// create an instance of WebApplicationBuilder class.
var builder = WebApplication.CreateBuilder(args);
// to access configuration settings stored in appsettings.json file, dotnet user-secrets...
var config = builder.Configuration;

// Add services to dependency injection container.
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// registers services that expose information about our endpoints, 
// used by Swagger and any other API documentation tool to generate documentation for our API 

// Give access to current HTTP request and response context.
builder.Services.AddHttpContextAccessor();

// Configuring Swagger to generate domcumentation for the API.
// Defining security requirements for JWT authentication.
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header. Format: 'Bearer your-token-key'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        }
    );
});

// Allowing access from any source in cors (developmen purpouse),
// is a better practice to specify the url address allowed to access the server.
builder.Services.AddCors(
    options =>
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        })
);

// config is used to read from user-secrets ---> json object ---> local development.
// GetEnvironment... to read enviroment variables --> Remote execution, Railways for example.
var jwtKey = config["JwtSettings__Key"] ?? Environment.GetEnvironmentVariable("JwtSettings__Key")!;
builder.Services.AddScoped<IJWTService>(provider =>
  {
      var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
      var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
      return new JWTService(jwtKey!, httpContextAccessor, mapper);
  }
);

builder.Services.AddScoped<IHassingService, HassingService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddScoped<IBidRepository, BidRepository>();

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddDbContext<GreenBayDbContext>(option =>
{
    var connectionString =
        Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
        ?? config["ConnectionStrings__DefaultConnection"];
    option.UseNpgsql(connectionString);
});

// Configuring authentication with JWT token.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidIssuer = config["JwtSettings:Issuer"],
            //ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // This ensures that migrations are only run during development, not during tests.
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<GreenBayDbContext>();
        dbContext.Database.Migrate();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// configuring http pipeline
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();
app.MapControllers();

app.Run();

public partial class Program
{
}
