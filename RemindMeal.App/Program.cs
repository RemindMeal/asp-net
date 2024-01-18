using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using RemindMealData.Models;
using RemindMeal.Services;
using Microsoft.AspNetCore.HttpOverrides;
using RemindMealData.DependencyInjection;
using RemindMealData.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Database
builder.Services.AddRemindMealDbContext(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("db");
    options.UseNpgsql(connectionString);
});

// Exception filter
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity
builder.Services
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
    .AddRemindMealEntityFrameworkStores();

builder.Services
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
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// User Resolver
builder.Services.AddRemindMealDataServices();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Console.WriteLine($"Startup: Environment is {app.Environment.EnvironmentName}");
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Using Developer Page and DB error page");
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    MigrationsServices.DBMigrate(app.Services);
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

app.MapRazorPages();

app.Run();

public partial class Program { }