using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemindMeal.Services;

namespace RemindMealData.DependencyInjection;

public static class ServicesInjectionExtensions
{
    public static IServiceCollection AddRemindMealDbContext(
        this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null)
    {
        return serviceCollection.AddDbContextFactory<RemindMealContext>(optionsAction);
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