using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemindMeal.Models;
using RemindMeal.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;

namespace RemindMeal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Database
            services.AddDbContext<RemindMealContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("db");
                options.UseNpgsql(connectionString);
            });

            // Exception filter
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity
            services
                .AddDefaultIdentity<User>(options => { 
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddErrorDescriber<RemindMealIdentityErrorDescriber>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<RemindMealContext>();
            
            services
                .AddRazorPages()
                .AddMvcOptions(options =>
                {
                    // Authentication by default on all pages
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                });

            // AutoMapper for mapping between Models <--> ViewModels
            services.AddAutoMapper(typeof(Startup));

            // User Resolver
            services.AddSingleton<IUserResolverService, UserResolverService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RemindMealContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Console.WriteLine($"Startup: Environment is {env.EnvironmentName}");
            if (env.IsDevelopment())
            {
                Console.WriteLine("Using Developer Page and DB error page");
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                var db = app.ApplicationServices.GetRequiredService<RemindMealContext>();
                db.Database.Migrate();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });

        }
    }
}
