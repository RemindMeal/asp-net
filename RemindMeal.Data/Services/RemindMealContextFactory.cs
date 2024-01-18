using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RemindMealData;

internal class RemindMealContextFactory : IDesignTimeDbContextFactory<RemindMealContext>
{
    public RemindMealContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RemindMealContext>();
        var connectionString = configuration.GetConnectionString("db");
        builder.UseNpgsql(connectionString);

        return new RemindMealContext(builder.Options);
    }
}
