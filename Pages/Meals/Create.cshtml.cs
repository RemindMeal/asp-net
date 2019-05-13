using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RemindMeal.Data;
using RemindMeal.Models;

namespace RemindMeal.Pages.Meals
{
    public sealed class CreateModel : PageModel
    {
        private readonly RemindMealContext _context;

        public CreateModel(RemindMealContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            AvailableFriends = new SelectList(_context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
            AvailableRecipes = new SelectList(_context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));
            return Page();
        }

        [BindProperty]
        public Meal Meal { get; set; }
        [BindProperty]
        public int[] SelectedFriends { get; set; }
        public SelectList AvailableFriends { get; set; }
        [BindProperty]
        public int[] SelectedRecipes { get; set; }
        public SelectList AvailableRecipes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Meals.Add(Meal);
            foreach (var friendId in SelectedFriends)
            {
                var friend = _context.Friends.Find(friendId);
                Meal.Presences.Add(new Presence {Meal = Meal, Friend = friend});
            }

            foreach (var recipeId in SelectedRecipes)
            {
                var recipe = _context.Recipes.Find(recipeId);
                Meal.Cookings.Add(new Cooking {Meal = Meal, Recipe = recipe});
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}