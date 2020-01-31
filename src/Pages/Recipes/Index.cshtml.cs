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
    public sealed class IndexModel : PageModel
    {
        private readonly RemindMealContext _context;

        public IndexModel(RemindMealContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; }

        public async Task OnGetAsync()
        {
            var recipes = from m in _context.Recipes select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                var lowerSearchString = SearchString.ToLower();
                recipes = recipes.Where(r => r.Name.ToLower().Contains(lowerSearchString) || r.Description.ToLower().Contains(lowerSearchString));
            }
            Recipe = await recipes.ToListAsync();
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
    }
}