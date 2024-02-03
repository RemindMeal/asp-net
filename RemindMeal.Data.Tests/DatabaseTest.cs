using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Data.Tests;

public class DatabaseTest
{
    readonly User user = new();

    ImmutableArray<Friend> CreateFriends() =>
    [
        new Friend
            {
                Name = "Anita",
                Surname = "Donea",
                User = user
            },
        new Friend
        {
            Name = "Françoise",
            Surname = "Lafont",
            User = user
        },
        new Friend
        {
            Name = "Brigitte",
            Surname = "Vernière",
            User = user
        }
    ];

    RemindMealContext CreateContext()
    {
        return new RemindMealContext(
            new DbContextOptionsBuilder<RemindMealContext>()
                .UseSqlite("Data Source=TestDatabase.db")
                .Options,
            new UserResolverServiceForTest(user)
        );
    }

    ImmutableArray<Recipe> CreateRecipes() =>
    [
        new Recipe
            {
                Name = "Poulet aux olives",
                Description = "Ben poulet avec des olives",
                Type = RecipeType.Main,
                User = user
            },
        new Recipe
        {
            Name = "Salade de noix",
            Description = string.Empty,
            Type = RecipeType.Starter,
            User = user
        },
        new Recipe
        {
            Name = "Tiramisu",
            Description = "Dessert Italien à la crème de mascarpone et au café.",
            Type = RecipeType.Dessert,
            User = user
        },
    ];
    
    [Fact]
    public void CreateFriend()
    {
        var Friends = CreateFriends();
        using (var context = CreateContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Friends.AddRange(CreateFriends());
            context.SaveChanges();
        }

        using (var context = CreateContext())
        {
            var friends = context.Friends.ToList();
            Assert.Equal(Friends.Length, friends.Count);
            foreach (var (expected, actual) in Friends.Zip(friends))
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Surname, actual.Surname);
            }
        }
    }

    [Fact]
    public void CreateRecipe()
    {
        var Recipes = CreateRecipes();
        using (var context = CreateContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Recipes.AddRange(Recipes);
            context.SaveChanges();   
        }

        using (var context = CreateContext())
        {
            var recipes = context.Recipes.ToList();
            Assert.Equal(Recipes.Length, recipes.Count);
            foreach (var (expected, actual) in Recipes.Zip(recipes))
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
                Assert.Equal(expected.Type, actual.Type);
            }
        }
    }

    [Fact]
    public void CreateMeal()
    {
        var Friends = CreateFriends();
        var Recipes = CreateRecipes();
        var date = DateTime.Now;
        var recipesToAdd = new[] {0, 1}.Select(i => Recipes[i]).ToList();
        var friendsToAdd = new[] {0, 1, 2}.Select(i => Friends[i]).ToList();
        using (var context = CreateContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var meal = new Meal
            {
                Date = date,
                User = user
            };

            foreach (var recipe in recipesToAdd)
            {
                meal.Cookings.Add(new Cooking
                {
                    Recipe = recipe,
                    Meal = meal
                });
            }

            foreach (var friend in friendsToAdd)
            {
                meal.Presences.Add(new Presence
                {
                    Friend = friend,
                    Meal = meal
                });
            }

            context.Add(meal);
            Assert.Equal(12, context.SaveChanges());
        }

        using (var context = CreateContext())
        {
            var meals = context.Meals
                .Include(m => m.Cookings)
                .ThenInclude(c => c.Recipe)
                .Include(m => m.Presences)
                .ThenInclude(p => p.Friend)
                .ToList();
            var meal = Assert.Single(meals);
            Assert.NotNull(meal);
            Assert.Equal(date, meal.Date);
            var friends = meal.Presences.Select(p => p.Friend).ToHashSet();
            foreach (var friend in friendsToAdd)
            {
                var friendDb = context.Friends.Single(f => f.Name == friend.Name && f.Surname == friend.Surname);
                Assert.Contains(friendDb, friends);
            }

            var recipes = meal.Cookings.Select(c => c.Recipe).ToHashSet();
            foreach (var recipe in recipesToAdd)
            {
                var recipeDb = context.Recipes.Single(r => r.Name == recipe.Name);
                Assert.Contains(recipeDb, recipes);
            }
        }
    }
}