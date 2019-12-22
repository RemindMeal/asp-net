using System.Linq;
using Microsoft.AspNetCore.Http;
using RemindMeal.Data;
using RemindMeal.Models;

namespace RemindMeal.Services
{
    public interface IUserResolverService
    {
        User GetCurrentSessionUser(RemindMealContext context);
    }
    
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentSessionUser(RemindMealContext context)
        {
            string userName = _httpContextAccessor.HttpContext.User?.Identity?.Name;
            return context.Users.SingleOrDefault(user => user.UserName == userName);
        }
    }
}