using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RemindMeal.Data;
using RemindMeal.Models;

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

        public IList<FriendIndexView> Friends { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Friends = await _context
                .Friends
                .Where(friend => friend.User == user)
                .Include(f => f.Presences)
                .ThenInclude(p => p.Meal)
                .ThenInclude(m => m.Cookings)
                .Select(friend => new FriendIndexView
                {
                    Id = friend.Id,
                    Name = friend.Name,
                    Surname = friend.Surname,
                    Meals = friend.Presences.Select(p => p.Meal).ToImmutableArray(),
                    RecipesCount = friend.Presences
                        .Select(p => p.Meal)
                        .Select(m => m.Cookings)
                        .Distinct()
                        .Count()
                })
                .ToListAsync();
        }
    }
}
