using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Meals;

public class DeleteModel : BaseDeleteModel
{
    public DeleteModel(RemindMealContext context) : base(context)
    {
    }

    [BindProperty]
    public Meal Meal { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Meal = await Context.Meals.FirstOrDefaultAsync(m => m.Id == id);

        if (Meal == null)
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

        Meal = await Context.Meals.FindAsync(id);

        if (Meal != null)
        {
            Context.Meals.Remove(Meal);
            await Context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
