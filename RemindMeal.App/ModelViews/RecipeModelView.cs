using System.ComponentModel.DataAnnotations;
using RemindMealData.Models;

namespace RemindMeal.ModelViews
{
    public sealed class RecipeModelView
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Type")]
        public RecipeType Type { get; set; }
    }
}