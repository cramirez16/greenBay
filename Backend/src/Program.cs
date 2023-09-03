using System.Text;
using System.Text.Json.Serialization;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using src.Data;
using src.helpers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddCors(
    options =>
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        })
);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddTransient<JWTHandler>();

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddDbContext<GreenBayDbContext>(option =>
{
    var connectionString =
        Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection")
        ?? config["ConnectionStrings:DefaultConnection"];
    option.UseNpgsql(connectionString);
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();

public partial class Program
{
}
