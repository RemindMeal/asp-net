using Microsoft.AspNetCore.Mvc;
using RemindMealData.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Recipes;

public sealed class CreateModel : BaseCreateModel
{
    public IActionResult OnGet()
    {
        RecipeModelView = new RecipeModelView();
        return Page();
    }

    [BindProperty]
    public RecipeModelView RecipeModelView { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var recipe = Mapper.Map<Recipe>(RecipeModelView);
        Context.Recipes.Add(recipe);
        await Context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
