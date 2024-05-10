using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemindMealData.Models;
using RemindMealData.Services;

namespace RemindMealData.DependencyInjection;

public static class ServicesInjectionExtensions
{
    public static IServiceCollection AddRemindMealDbContext(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json")
            .Build();
            

        var connectionString = configuration.GetConnectionString("db");
        Console.WriteLine($"connectionString = {connectionString}");        
        return serviceCollection.AddDbContextFactory<RemindMealContext>(options => options.UseNpgsql(connectionString), lifetime);
    }

    public static IdentityBuilder AddRemindMealEntityFrameworkStores(this IdentityBuilder builder)
    {
        return builder.AddEntityFrameworkStores<RemindMealContext>();
    }

    public static IServiceCollection AddRemindMealDataUserResolverService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IUserResolverService, UserResolverService>();
    }

    public static IServiceCollection AddRemindMealDataDbSetProviders(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IDbSetProvider<Friend>, FriendDbSetProvider>()
            .AddSingleton<IDbSetProvider<Recipe>, RecipeDbSetProvider>()
        ;
    }
}