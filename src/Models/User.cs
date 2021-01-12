using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RemindMeal.Models
{
    public sealed class User : IdentityUser
    {
        public User()
        {
            Recipes = new List<Recipe>();
        }

        public IList<Recipe> Recipes { get; set; }
        
        public override string ToString()
        {
            return $"User {UserName}";
        }
    }
}