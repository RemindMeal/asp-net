using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;

namespace RemindMeal.Pages.Meals
{
    public class IndexModel : PageModel
    {
        private readonly RemindMealContext _context;

        public IndexModel(RemindMealContext context)
        {
            _context = context;
        }

        public IList<Meal> Meals { get; set; }

        public async Task OnGetAsync()
        {
            Meals = await _context.Meals
                .Include(m => m.Presences)
                .ThenInclude(p => p.Friend)
                .Include(m => m.Cookings)
                .ThenInclude(c => c.Recipe)
                .ToListAsync();
        }
    }
}
