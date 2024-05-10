using RemindMealData;
using RemindMealData.Models;
using RemindMealData.Services;

namespace RemindMeal.Data.Tests;

public class UserResolverServiceForTest(User user) : IUserResolverService
{
    public User GetCurrentSessionUser(RemindMealContext context) => user;
};
