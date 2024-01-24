using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemindMealData;

namespace RemindMeal.App.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // remove the existing context configuration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RemindMealContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContextFactory<RemindMealContext>(options =>
                options.UseSqlite("Data Source=TestDatabase.db"));
        });
        builder.UseEnvironment("Development");
    }
}