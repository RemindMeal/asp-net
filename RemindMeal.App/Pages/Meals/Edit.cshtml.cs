using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals;

public class EditModel : BaseEditModel
{
    public EditModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
    {
    }

    [BindProperty]
    public MealModelView MealMV { get; set; }

    public IActionResult OnGet(int? id)
    {
        if (id == null) return NotFound();

        var meal = Context.Meals
            .Include(m => m.Presences)
            .Include(m => m.Cookings)
            .Single(m => m.Id == id);
        MealMV = Mapper.Map<MealModelView>(meal);
        MealMV.AvailableFriends = new SelectList(Context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
        MealMV.AvailableRecipes = new SelectList(Context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));

        if (MealMV == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var meal = Context.Meals
            .Include(m => m.Cookings)
            .Include(m => m.Presences)
            .Single(m => m.Id == MealMV.Id);

        meal.Date = MealMV.Date;
        var recipeIds = meal.Cookings.Select(c => c.RecipeId).ToArray();
        Context.AddRange(
            MealMV.SelectedRecipeIds
                .Where(recipeId => !recipeIds.Contains(recipeId))
                .Select(recipeId => new Cooking { MealId = meal.Id, RecipeId = recipeId })
        );
        Context.RemoveRange(meal.Cookings.Where(c => !MealMV.SelectedRecipeIds.Contains(c.RecipeId)));

        var friendIds = meal.Presences.Select(c => c.FriendId).ToArray();
        Context.AddRange(
            MealMV.SelectedFriendIds
                .Where(friendId => !friendIds.Contains(friendId))
                .Select(friendId => new Presence { MealId = meal.Id, FriendId = friendId })
        );
        Context.RemoveRange(meal.Presences.Where(p => !MealMV.SelectedFriendIds.Contains(p.FriendId)));

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MealExists(meal.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool MealExists(int id)
    {
        return Context.Meals.Any(e => e.Id == id);
    }
}
