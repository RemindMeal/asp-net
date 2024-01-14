using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends;

public sealed class IndexModel : ReadModel
{
    public IndexModel(RemindMealContext context) : base(context)
    {
    }

    public IList<Friend> Friends { get;set; }

    public async Task OnGetAsync()
    {
        Friends = await Context.Friends.ToListAsync();
    }
}
