using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RemindMeal.Data;
using RemindMeal.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals
{
    public sealed class CreateModel : PageModel
    {
        private readonly RemindMealContext _context;
        private readonly IMapper _mapper;

        public CreateModel(RemindMealContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            AvailableFriends = new SelectList(_context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
            AvailableRecipes = new SelectList(_context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));
            return Page();
        }

        [BindProperty]
        public MealModelView Meal { get; set; }

        [BindProperty]
        public int[] SelectedFriends { get; set; }
        public SelectList AvailableFriends { get; set; }

        [BindProperty]
        public int[] SelectedRecipes { get; set; }
        public SelectList AvailableRecipes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var meal = _mapper.Map<Meal>(Meal);
            _context.Meals.Add(meal);
            foreach (var friendId in SelectedFriends)
            {
                var friend = _context.Friends.Find(friendId);
                meal.Presences.Add(new Presence { Meal = meal, Friend = friend });
            }

            foreach (var recipeId in SelectedRecipes)
            {
                var recipe = _context.Recipes.Find(recipeId);
                meal.Cookings.Add(new Cooking { Meal = meal, Recipe = recipe });
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}