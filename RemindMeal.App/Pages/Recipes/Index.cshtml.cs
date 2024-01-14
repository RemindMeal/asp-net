using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.Structures;

namespace RemindMeal.Pages.Recipes;

public sealed class IndexModel : ReadModel
{   
    public IndexModel(RemindMealContext context) : base(context)
    {
    }

    public IList<Recipe> Recipes { get; set; }

    // Sort
    public SortOrder NameSort { get; set; }
    public string NameSortLink => NameSort.Invert().ConvertToString();

    // Search
    [BindProperty(SupportsGet = true)]
    public string SearchString { get; set; }

    public async Task OnGetAsync(string nameSortOrder)
    {
        var recipes = from m in Context.Recipes select m;
        
        if (!string.IsNullOrEmpty(SearchString))
        {
            var lowerSearchString = SearchString.ToLower();
            recipes = recipes.Where(r => 
                r.Name.ToLower().Contains(lowerSearchString) ||
                r.Description.ToLower().Contains(lowerSearchString));
        }

        NameSort = nameSortOrder.ToSortOrder();

        recipes = NameSort == SortOrder.Descending
            ? recipes.OrderByDescending(r => r.Name)
            : recipes.OrderBy(r => r.Name);

        Recipes = await recipes.ToListAsync();
    }   
}