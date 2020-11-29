using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;

namespace RemindMeal.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly RemindMealContext _context;

        public DetailsModel(RemindMealContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; }
        
        public IEnumerable<(Friend, int)> Friends =>
            Recipe
                .Cookings
                .SelectMany(cooking => cooking.Meal.Presences.Select(p => p.Friend))
                .GroupBy(friend => friend)
                .Select(g => (g.Key, g.Count()));

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
