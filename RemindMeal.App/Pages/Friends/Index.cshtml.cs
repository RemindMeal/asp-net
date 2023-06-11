using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMealData;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends
{
    public sealed class IndexModel : PageModel
    {
        private readonly RemindMealContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(RemindMealContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Friend> Friends { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Friends = await _context
                .Friends
                .Where(friend => friend.User == user)
                .ToListAsync();
        }
    }
}
