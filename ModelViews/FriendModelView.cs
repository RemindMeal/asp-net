using System.ComponentModel.DataAnnotations;

namespace RemindMeal.ModelViews
{
    public sealed class FriendModelView
    {
        [Display(Name = "Prénom")]
        public string Name { get; set; }
        
        [Display(Name = "Nom")]
        public string Surname { get; set; }   
    }
}