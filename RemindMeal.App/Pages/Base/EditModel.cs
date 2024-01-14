using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;

namespace RemindMeal.Pages;

public class BaseEditModel : PageModel
{
    public BaseEditModel(RemindMealContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    protected RemindMealContext Context { get; init; }
    protected IMapper Mapper { get; init; }
}
