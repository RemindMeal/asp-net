using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends;

public sealed class EditModel : BaseEditModel
{
    public EditModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Context.Attach(Friend).State = EntityState.Modified;

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FriendExists(Friend.Id))
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

    private bool FriendExists(int id)
    {
        return Context.Friends.Any(e => e.Id == id);
    }
}
