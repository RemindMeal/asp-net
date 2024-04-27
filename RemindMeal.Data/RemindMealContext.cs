using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RemindMealData.Models;
using RemindMeal.Services;
using Microsoft.Extensions.Configuration;

namespace RemindMealData;

public class RemindMealContext : IdentityDbContext<User>
{
    private readonly IUserResolverService _userResolverService;

    public RemindMealContext(DbContextOptions<RemindMealContext> options)
        : base(options)
    {}

    public RemindMealContext(DbContextOptions<RemindMealContext> options, IUserResolverService userResolverService)
        : base(options)
    {
        _userResolverService = userResolverService;
    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Presence> Participations { get; set; }
    public DbSet<Cooking> Cookings { get; set; }

    public User GetCurrentUser()
    {
        return _userResolverService.GetCurrentSessionUser(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Meal <--> Recipe via Cooking
        modelBuilder.Entity<Cooking>().HasKey(x => new { x.MealId, x.RecipeId });
        modelBuilder.Entity<Cooking>()
            .HasOne(cooking => cooking.Meal)
            .WithMany(meal => meal.Cookings)
            .HasForeignKey(cooking => cooking.MealId);
        modelBuilder.Entity<Cooking>()
            .HasOne(cooking => cooking.Recipe)
            .WithMany(recipe => recipe.Cookings)
            .HasForeignKey(cooking => cooking.RecipeId);

        // Meal <--> Friend via Participation
        modelBuilder.Entity<Presence>().HasKey(p => new { p.MealId, p.FriendId });
        modelBuilder.Entity<Presence>()
            .HasOne(p => p.Friend)
            .WithMany(f => f.Presences)
            .HasForeignKey(p => p.FriendId);
        modelBuilder.Entity<Presence>()
            .HasOne(p => p.Meal)
            .WithMany(m => m.Presences)
            .HasForeignKey(p => p.MealId);

        // RecipeType
        modelBuilder.Entity<Recipe>()
            .Property(r => r.Type)
            .HasConversion(new EnumToStringConverter<RecipeType>());

        modelBuilder.Entity<Recipe>().HasQueryFilter(r => r.User.Id == GetCurrentUser().Id);
        modelBuilder.Entity<Friend>().HasQueryFilter(f => f.User.Id == GetCurrentUser().Id);
        modelBuilder.Entity<Meal>().HasQueryFilter(m => m.User.Id == GetCurrentUser().Id);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var user = GetCurrentUser();
        if (user is not null)
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.ProcessCreation(user);
        }
        return await base.SaveChangesAsync(true, cancellationToken);
    }
}

internal static class ChangeTrackerExtensions
{
    public static void ProcessCreation(this ChangeTracker changeTracker, User user)
    {
        foreach (var item in changeTracker.Entries<IHasUser>().Where(e => e.State == EntityState.Added))
        {
            item.Entity.User = user;
        }
    }
}
