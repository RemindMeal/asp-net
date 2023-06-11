using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RemindMealData.Services;

public static class MigrationsServices
{
    public static void DBMigrate(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<RemindMealContext>();
        db.Database.Migrate();
    }
}