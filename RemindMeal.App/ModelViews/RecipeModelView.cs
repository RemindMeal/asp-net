using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public Category Type { get; set; }

        [Display(Name = "Categories")]
        public SelectList Categories { get; set; }
    }
}