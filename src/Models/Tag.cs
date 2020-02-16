using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Tag : IHasUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        
        public User User { get; set; }
        
        public ICollection<RecipeTag> RecipeTags { get; set; }
    }
}