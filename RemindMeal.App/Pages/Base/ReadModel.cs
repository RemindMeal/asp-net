using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages;

public class ReadModel : PageModel
{
    public ReadModel(RemindMealContext context)
    {
        Context = context;
    }

    protected RemindMealContext Context { get; init; }
}
