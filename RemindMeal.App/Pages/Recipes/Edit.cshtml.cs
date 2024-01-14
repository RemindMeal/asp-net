using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Recipes;

public sealed class EditModel : BaseEditModel
{
    public EditModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
    {
    }

    [BindProperty]
    public RecipeModelView RecipeView { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await Context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
        RecipeView = Mapper.Map<RecipeModelView>(recipe);

        if (RecipeView == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var recipeId = RecipeView.Id;
        var recipe = await Context.Recipes.SingleAsync(r => r.Id == recipeId);

        recipe.Name = RecipeView.Name;
        recipe.Description = RecipeView.Description;
        recipe.Type = RecipeView.Type;

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecipeExists(RecipeView.Id)) return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool RecipeExists(int id)
    {
        return Context.Recipes.Any(e => e.Id == id);
    }
}
