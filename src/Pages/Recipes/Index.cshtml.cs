using System;
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

        public IList<Recipe> Recipes { get; set; }

        // Sort
        public SortOrder NameSort { get; set; }
        public string NameSortLink => Convert(Invert(NameSort));

        // Search
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string nameSortOrder)
        {
            var recipes = from m in _context.Recipes select m;
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                var lowerSearchString = SearchString.ToLower();
                recipes = recipes.Where(r => 
                    r.Name.ToLower().Contains(lowerSearchString) ||
                    r.Description.ToLower().Contains(lowerSearchString));
            }

            NameSort = Convert(nameSortOrder);

            recipes = NameSort == SortOrder.Descending
                ? recipes.OrderByDescending(r => r.Name)
                : recipes.OrderBy(r => r.Name);

            Recipes = await recipes
                .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
                .ToListAsync();
        }

        public enum SortOrder
        {
            Ascending,
            Descending
        }
        
        private static SortOrder Invert(SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    return SortOrder.Descending;
                case SortOrder.Descending:
                    return SortOrder.Ascending;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }
        }

        private static SortOrder Convert(string sortOrderString)
        {
            return sortOrderString == "desc" ? SortOrder.Descending : SortOrder.Ascending;
        }

        private static string Convert(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "asc";
                case SortOrder.Descending:
                    return "desc";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null);
            }
        }
    }
}