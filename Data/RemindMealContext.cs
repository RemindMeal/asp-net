using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Models;

namespace RemindMeal.Data
{
    public class RemindMealContext : IdentityDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Presence> Participations { get; set; }
        public DbSet<Cooking> Cookings { get; set; }

        public RemindMealContext(DbContextOptions<RemindMealContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)    
        {                                  
            base.OnModelCreating(modelBuilder);
            // Meal <--> Recipe via Cooking
            modelBuilder.Entity<Cooking>().HasKey(x => new {x.MealId, x.RecipeId});
            modelBuilder.Entity<Cooking>()
                .HasOne(cooking => cooking.Meal)
                .WithMany(meal => meal.Cookings)
                .HasForeignKey(cooking => cooking.MealId);
            modelBuilder.Entity<Cooking>()
                .HasOne(cooking => cooking.Recipe)
                .WithMany(recipe => recipe.Cookings)
                .HasForeignKey(cooking => cooking.RecipeId);
            
            // Meal <--> Friend via Participation
            modelBuilder.Entity<Presence>().HasKey(p => new {p.MealId, p.FriendId});
            modelBuilder.Entity<Presence>()
                .HasOne(p => p.Friend)
                .WithMany(f => f.Presences)
                .HasForeignKey(p => p.FriendId);
            modelBuilder.Entity<Presence>()
                .HasOne(p => p.Meal)
                .WithMany(m => m.Presences)
                .HasForeignKey(p => p.MealId);
        }
    }
}
