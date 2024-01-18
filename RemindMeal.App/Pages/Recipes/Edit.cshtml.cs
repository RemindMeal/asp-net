using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMeal.ModelViews;

namespace RemindMeal.Pages.Recipes
{
    public sealed class EditModel : PageModel
    {
        private readonly RemindMealContext _context;
        private readonly IMapper _mapper;

        public EditModel(RemindMealContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public RecipeModelView RecipeView { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
            RecipeView = _mapper.Map<RecipeModelView>(recipe);

            if (RecipeView == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var recipeId = RecipeView.Id;
            var recipe = await _context.Recipes.SingleAsync(r => r.Id == recipeId);
            
            _context.Attach(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(RecipeView.Id)) return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
