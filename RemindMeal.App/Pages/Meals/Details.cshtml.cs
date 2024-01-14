using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals;

public class DetailsModel : ReadModel
{
    public DetailsModel(RemindMealContext context, IMapper mapper) : base(context)
    {
        Mapper = mapper;
    }

    private IMapper Mapper { get; init; }

    public MealModelView Meal { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var meal = await Context
            .Meals
            .Include(m => m.Presences)
            .ThenInclude(presence => presence.Friend)
            .Include(m => m.Cookings)
            .ThenInclude(cooking => cooking.Recipe)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (meal == null)
        {
            return NotFound();
        }
        
        Meal = Mapper.Map<MealModelView>(meal);

        return Page();
    }
}
