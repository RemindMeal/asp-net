using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals;

public sealed class CreateModel : BaseCreateModel
{
    public CreateModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public IActionResult OnGet()
    {
        MealView = new MealModelView
        {
            AvailableFriends = new SelectList(Context.Friends, nameof(Friend.Id), nameof(Friend.FullName)),
            AvailableRecipes = new SelectList(Context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name))
        };
        return Page();
    }

    [BindProperty]
    public MealModelView MealView { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var meal = Mapper.Map<Meal>(MealView);
        Context.Update(meal);
        await Context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
