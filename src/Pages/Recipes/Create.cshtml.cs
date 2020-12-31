using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMeal.Data;
using RemindMeal.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Recipes
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
            RecipeModelView = new RecipeModelView();
            return Page();
        }

        [BindProperty]
        public RecipeModelView RecipeModelView { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recipe = _mapper.Map<Recipe>(RecipeModelView);
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}