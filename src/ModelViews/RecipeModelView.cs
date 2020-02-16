using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RemindMeal.Models;

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

        [Display(Name = "Tags")]
        public ICollection<int> SelectedTagIds { get; set; } = new List<int>();
        public SelectList AvailableTags { get; set; }
    }
}