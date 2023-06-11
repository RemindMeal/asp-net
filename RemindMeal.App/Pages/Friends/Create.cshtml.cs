using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends
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
        public FriendView FriendMV { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var friend = _mapper.Map<Friend>(FriendMV);
            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}