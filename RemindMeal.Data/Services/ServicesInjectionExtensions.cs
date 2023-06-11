using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemindMeal.Services;

namespace RemindMealData.DependencyInjection;

public static class ServicesInjectionExtensions
{
    public static IServiceCollection AddRemindMealDbContext(
        this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    {
        return serviceCollection.AddDbContext<RemindMealContext>(optionsAction, contextLifetime, optionsLifetime);
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