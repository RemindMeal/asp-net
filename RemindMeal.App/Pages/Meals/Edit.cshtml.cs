using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Meals
{
    public class EditModel : PageModel
    {
        private readonly RemindMealContext _context;
        private readonly IMapper mapper;

        public EditModel(RemindMealContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [BindProperty]
        public MealModelView MealMV { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            var meal = _context.Meals
                .Include(m => m.Presences)
                .Include(m => m.Cookings)
                .Single(m => m.Id == id);
            MealMV = mapper.Map<MealModelView>(meal);
            MealMV.AvailableFriends = new SelectList(_context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
            MealMV.AvailableRecipes = new SelectList(_context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));

            if (MealMV == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var meal = _context.Meals
                .Include(m => m.Cookings)
                .Include(m => m.Presences)
                .Single(m => m.Id == MealMV.Id);

            meal.Date = MealMV.Date;
            var recipeIds = meal.Cookings.Select(c => c.RecipeId).ToArray();
            _context.AddRange(
                MealMV.SelectedRecipeIds
                    .Where(recipeId => !recipeIds.Contains(recipeId))
                    .Select(recipeId => new Cooking { MealId = meal.Id, RecipeId = recipeId })
            );
            _context.RemoveRange(meal.Cookings.Where(c => !MealMV.SelectedRecipeIds.Contains(c.RecipeId)));

            var friendIds = meal.Presences.Select(c => c.FriendId).ToArray();
            _context.AddRange(
                MealMV.SelectedFriendIds
                    .Where(friendId => !friendIds.Contains(friendId))
                    .Select(friendId => new Presence { MealId = meal.Id, FriendId = friendId })
            );
            _context.RemoveRange(meal.Presences.Where(p => !MealMV.SelectedFriendIds.Contains(p.FriendId)));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealExists(meal.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.Id == id);
        }
    }
}
