using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;
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

        [BindProperty]
        public int[] SelectedFriends { get; set; }
        public SelectList AvailableFriends { get; set; }

        [BindProperty]
        public int[] SelectedRecipes { get; set; }
        public SelectList AvailableRecipes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(m => m.Presences).ThenInclude(p => p.Friend)
                .Include(m => m.Cookings).ThenInclude(c => c.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            MealMV = mapper.Map<MealModelView>(meal);
            SelectedFriends = meal.Friends.Select(friend => friend.Id).ToArray();
            SelectedRecipes = meal.Recipes.Select(recipe => recipe.Id).ToArray();
            AvailableFriends = new SelectList(_context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
            AvailableRecipes = new SelectList(_context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));

            if (MealMV == null)
            {
                return NotFound();
            }
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
                .First(m => m.Id == MealMV.Id);

            meal.Date = MealMV.Date;
            var recipeIds = meal.Cookings.Select(c => c.RecipeId).ToArray();
            _context.AddRange(
                SelectedRecipes
                    .Where(recipeId => !recipeIds.Contains(recipeId))
                    .Select(recipeId => new Cooking { MealId = meal.Id, RecipeId = recipeId })
            );
            _context.RemoveRange(meal.Cookings.Where(c => !SelectedRecipes.Contains(c.RecipeId)));

            var friendIds = meal.Presences.Select(c => c.FriendId).ToArray();
            _context.AddRange(
                SelectedFriends
                    .Where(friendId => !friendIds.Contains(friendId))
                    .Select(friendId => new Presence { MealId = meal.Id, FriendId = friendId })
            );
            _context.RemoveRange(meal.Presences.Where(p => !SelectedFriends.Contains(p.FriendId)));

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
