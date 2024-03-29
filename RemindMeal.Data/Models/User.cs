using Microsoft.AspNetCore.Identity;

namespace RemindMealData.Models;

public sealed class User : IdentityUser
{
    public IList<Recipe> Recipes { get; set; } = [];

    public override string ToString()
    {
        return $"User {UserName}";
    }
}
