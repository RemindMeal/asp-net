using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.ModelViews;
using RemindMeal.Pages.Friends;

namespace RemindMeal.Pages.Recipes;

public sealed class CreateModel : BaseCreateModel
{
    public CreateModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
    {
    }

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
