using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends;

public sealed class CreateModel : BaseEditModel
{
    public CreateModel(RemindMealContext context, IMapper mapper) : base(context, mapper)
    {}

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public FriendView FriendMV { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var friend = Mapper.Map<Friend>(FriendMV);
        Context.Friends.Add(friend);
        await Context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}