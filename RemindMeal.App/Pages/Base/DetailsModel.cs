using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages;

public class BaseDetailsModel : PageModel
{
    public BaseDetailsModel(RemindMealContext context)
    {
        Context = context;
    }

    protected RemindMealContext Context { get; init; }
}
