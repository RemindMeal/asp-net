using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends;

public sealed class DetailsModel : ReadModel
{
    public DetailsModel(RemindMealContext context) : base(context)
    {
    }

    public Friend Friend { get; set; }

    public IEnumerable<(Recipe, int)> Recipes =>
        Friend
        .Presences
        .SelectMany(presence => presence.Meal.Cookings.Select(c => c.Recipe))
        .GroupBy(recipe => recipe)
        .Select(g => (g.Key, g.Count()));


    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Friend = await Context
            .Friends
            .Include(friend => friend.Presences)
            .ThenInclude(presence => presence.Meal)
            .ThenInclude(meal => meal.Cookings)
            .ThenInclude(cooking => cooking.Recipe)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Friend == null)
        {
            return NotFound();
        }
        return Page();
    }
}
