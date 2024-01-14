using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages.Friends;

public class BaseCreateModel : PageModel
{
    public BaseCreateModel(RemindMealContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    protected RemindMealContext Context { get; init; }
    protected IMapper Mapper { get; init; }
}
