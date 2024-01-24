using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.Services;

public sealed class DatabaseTest
{
    private readonly DbContextOptions<RemindMealContext> _contextOptions = new DbContextOptionsBuilder<RemindMealContext>()
        .UseSqlite("Data Source=TestDatabase.db")
        .Options;
    private static readonly User User = new();
    private readonly IUserResolverService _userResolverService = new UserResolverServiceForTest(User);
    private static readonly ImmutableArray<Friend> Friends =
    [
        new Friend
            {
                Name = "Jesika",
                Surname = "Barisic",
                User = User
            },
        new Friend
        {
            Name = "Françoise",
            Surname = "Lafont",
            User = User
        },
        new Friend
        {
            Name = "Brigitte",
            Surname = "Vernière",
            User = User
        }
,
    ];

    private static readonly ImmutableArray<Recipe> Recipes =
    [
        new Recipe
            {
                Name = "Poulet aux olives",
                Description = "Ben poulet avec des olives",
                Type = RecipeType.Main,
                User = User
            },
        new Recipe
        {
            Name = "Salade de noix",
            Description = string.Empty,
            Type = RecipeType.Starter,
            User = User
        },
        new Recipe
        {
            Name = "Tiramisu",
            Description = "Dessert Italien à la crème de mascarpone et au café.",
            Type = RecipeType.Dessert,
            User = User
        },
    ];

    private class UserResolverServiceForTest(User user) : IUserResolverService
    {
        private readonly User _user = user;

        public User GetCurrentSessionUser(RemindMealContext context) => _user;
    }

    private RemindMealContext CreateContext() => new(_contextOptions, _userResolverService);
    
    [Fact]
    public void CreateFriend()
    {
        using (var context = CreateContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Friends.AddRange(Friends);
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
                User = User
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
            Assert.Equal(13, context.SaveChanges());
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