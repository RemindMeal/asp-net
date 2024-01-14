using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Recipes
{
    public class DetailsModel :  BaseDetailsModel
    {
        public DetailsModel(RemindMealContext context) : base(context)
        {
        }

        public Recipe Recipe { get; set; }
        
        public IEnumerable<(Friend, int)> Friends =>
            Recipe
                .Cookings
                .SelectMany(cooking => cooking.Meal.Presences.Select(p => p.Friend))
                .GroupBy(friend => friend)
                .Select(g => (g.Key, g.Count()));

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await Context
                .Recipes
                .Include(recipe => recipe.Cookings)
                .ThenInclude(cooking => cooking.Meal)
                .ThenInclude(meal => meal.Presences)
                .ThenInclude(presence => presence.Friend)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
