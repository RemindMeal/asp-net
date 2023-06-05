using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;
using RemindMeal.Structures;

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
        
        // Sort
        public SortOrder DateSort { get; set; }
        public string DateSortLink => DateSort.Invert().ConvertToString();

        public async Task OnGetAsync(string dateSortOrder)
        {
            var meals = from m in _context.Meals select m;

            DateSort = dateSortOrder.ToSortOrder();

            meals = DateSort == SortOrder.Descending
                ? meals.OrderByDescending(m => m.Date)
                : meals.OrderBy(m => m.Date);
            
            Meals = await meals
                .Include(m => m.Presences)
                .ThenInclude(p => p.Friend)
                .Include(m => m.Cookings)
                .ThenInclude(c => c.Recipe)
                .ToListAsync();
        }
    }
}
