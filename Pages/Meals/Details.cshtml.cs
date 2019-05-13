using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;

namespace RemindMeal.Pages.Meals
{
    public class DetailsModel : PageModel
    {
        private readonly RemindMealContext _context;

        public DetailsModel(RemindMealContext context)
        {
            _context = context;
        }

        public Meal Meal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Meal = await _context
                .Meals
                .Include(meal => meal.Presences)
                .ThenInclude(presence => presence.Friend)
                .Include(meal => meal.Cookings)
                .ThenInclude(cooking => cooking.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Meal == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
