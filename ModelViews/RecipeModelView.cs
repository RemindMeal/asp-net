using System.ComponentModel.DataAnnotations;

namespace RemindMeal.ModelViews
{
    public class RecipeModelView
    {   
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}