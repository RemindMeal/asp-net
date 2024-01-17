using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages;

public class BaseCreateModel : PageModel
{
    [Inject]
    protected RemindMealContext Context { get; init; }

    [Inject]
    protected IMapper Mapper { get; init; }
}
