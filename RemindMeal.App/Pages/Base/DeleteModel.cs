using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages;

public class BaseDeleteModel : PageModel
{
    public BaseDeleteModel(RemindMealContext context)
    {
        Context = context;
    }

    protected RemindMealContext Context { get; init; }
}
