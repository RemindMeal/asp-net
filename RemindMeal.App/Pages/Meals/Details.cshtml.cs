using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals
{
    public class DetailsModel : PageModel
    {
        private readonly RemindMealContext _context;
        private readonly IMapper _mapper;

        public DetailsModel(RemindMealContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MealModelView Meal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context
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
            
            Meal = _mapper.Map<MealModelView>(meal);

            return Page();
        }
    }
}
