using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMeal.Data;
using RemindMeal.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Recipes
{
    public class CreateModel : PageModel
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
            return Page();
        }

        [BindProperty]
        public RecipeModelView Recipe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recipe = _mapper.Map<Recipe>(Recipe);
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}