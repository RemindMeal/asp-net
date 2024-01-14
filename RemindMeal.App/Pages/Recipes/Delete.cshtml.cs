using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Recipes;

public class DeleteModel : BaseDeleteModel
{
    public DeleteModel(RemindMealContext context) : base(context)
    {
    }

    [BindProperty] public Recipe Recipe { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Recipe = await Context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

        if (Recipe == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Recipe = await Context.Recipes.FindAsync(id);

        if (Recipe != null)
        {
            Context.Recipes.Remove(Recipe);
            await Context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}