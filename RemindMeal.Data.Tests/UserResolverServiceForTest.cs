using RemindMealData;
using RemindMealData.Models;
using RemindMeal.Services;

namespace RemindMeal.Data.Tests;

public class UserResolverServiceForTest(User user) : IUserResolverService
{
    private readonly User _user = user;

    public User GetCurrentSessionUser(RemindMealContext context) => _user;
};
