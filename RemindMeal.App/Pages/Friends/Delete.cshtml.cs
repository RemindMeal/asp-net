using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends;

public class DeleteModel : BaseDeleteModel
{
    public DeleteModel(RemindMealContext context) : base(context)
    {
    }

    [BindProperty]
    public Friend Friend { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Friend = await Context.Friends.FirstOrDefaultAsync(m => m.Id == id);

        if (Friend == null)
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

        Friend = await Context.Friends.FindAsync(id);

        if (Friend != null)
        {
            Context.Friends.Remove(Friend);
            await Context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
