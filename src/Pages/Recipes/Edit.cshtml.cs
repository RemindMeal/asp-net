using System.Collections.Immutable;
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

            var recipe = await _context.Recipes.Include(r => r.RecipeTags).FirstOrDefaultAsync(m => m.Id == id);
            RecipeView = _mapper.Map<RecipeModelView>(recipe);
            RecipeView.AvailableTags = new SelectList(_context.Tags, nameof(Tag.Id), nameof(Tag.Name));

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
            var recipe = await _context.Recipes.Include(r => r.RecipeTags).SingleAsync(r => r.Id == recipeId);

            var previousTagIds = recipe.RecipeTags.Select(rt => rt.TagId).ToImmutableHashSet();
            var selectedTags = RecipeView.SelectedTagIds.ToImmutableHashSet();
            var newTagIds = selectedTags.Where(tagId => !previousTagIds.Contains(tagId));
            await _context.AddRangeAsync(newTagIds.Select(tagId => new RecipeTag{RecipeId = recipeId, TagId = tagId}));
            _context.RemoveRange(recipe.RecipeTags.Where(rt => !selectedTags.Contains(rt.TagId)));

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
