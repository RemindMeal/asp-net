[assembly: HostingStartup(typeof(RemindMeal.Areas.Identity.IdentityHostingStartup))]
namespace RemindMeal.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}