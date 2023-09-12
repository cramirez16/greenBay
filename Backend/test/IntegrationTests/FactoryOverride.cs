// -----------------------------------------------------
// version 1: Using Microsoft.EntityFrameworkCore.Sqlite
// -----------------------------------------------------
using System.Data.Common;
using src.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using src.Services.IServices;
using src.Services;


namespace BackendNUnitTest
{
    public class FactoryOverride : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<GreenBayDbContext>));
                if (dbContextDescriptor != null) { services.Remove(dbContextDescriptor); }
                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));
                if (dbContextDescriptor != null) { services.Remove(dbContextDescriptor); }
                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });
                services.AddDbContext<GreenBayDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<GreenBayDbContext>();
                context.Database.Migrate();
                services.AddScoped<IJWTService, JWTService>();
            }
            );
            builder.UseEnvironment("Development");
        }
    }
}