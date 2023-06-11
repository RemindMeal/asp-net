using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RemindMealData.Models;

namespace RemindMeal.Pages.Friends
{
    public class FriendView
    {
        [Display(Name = "Prénom")]
        public string Name { get; set; }
        
        [Display(Name = "Nom")]
        public string Surname { get; set; }
        
        [Display(Name = "Nom")]
        public string FullName => $"{Name} {Surname}";
    }
    
    public sealed class FriendIndexView : FriendView
    {
        public int Id { get; set; }
        
        public ICollection<Meal> Meals { get; set; }
        
        public int RecipesCount { get; set; }
    }
}