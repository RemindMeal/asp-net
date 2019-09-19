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
            MealMV = new MealModelView();
            MealMV.AvailableFriends = new SelectList(_context.Friends, nameof(Friend.Id), nameof(Friend.FullName));
            MealMV.AvailableRecipes = new SelectList(_context.Recipes, nameof(Recipe.Id), nameof(Recipe.Name));
            return Page();
        }

        [BindProperty]
        public MealModelView MealMV { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var meal = _mapper.Map<Meal>(MealMV);
            _context.Update(meal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}