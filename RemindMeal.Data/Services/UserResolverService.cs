using Microsoft.AspNetCore.Http;
using RemindMealData.Models;

namespace RemindMealData.Services;

public interface IUserResolverService
{
    User GetCurrentSessionUser(RemindMealContext context);
}

internal class UserResolverService(IHttpContextAccessor httpContextAccessor) : IUserResolverService
{
    public User GetCurrentSessionUser(RemindMealContext context)
    {
        string userName = httpContextAccessor.HttpContext.User?.Identity?.Name;
        return context.Users.SingleOrDefault(user => user.UserName == userName);
    }
}
