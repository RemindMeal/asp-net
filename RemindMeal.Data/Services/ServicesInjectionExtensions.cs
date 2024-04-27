using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemindMeal.Services;

namespace RemindMealData.DependencyInjection;

public static class ServicesInjectionExtensions
{
    public static IServiceCollection AddRemindMealDbContext(this IServiceCollection serviceCollection)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("db");
        Console.WriteLine($"connectionString = {connectionString}");        
        return serviceCollection.AddDbContext<RemindMealContext>(options => options.UseNpgsql(connectionString));
    }

    public static IdentityBuilder AddRemindMealEntityFrameworkStores(this IdentityBuilder builder)
    {
        return builder.AddEntityFrameworkStores<RemindMealContext>();
    }

    public static IServiceCollection AddRemindMealDataServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IUserResolverService, UserResolverService>();
    }
}