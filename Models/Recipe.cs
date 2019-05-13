using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Recipe
    {
        public Recipe()
        {
            CreationDate = DateTime.Now;
        }
        
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Date de création")]
        public DateTime CreationDate { get; }
        [Required]
        [Display(Name = "Type")]
        public RecipeType Type { get; }
        
        // Relationships
        public ICollection<Cooking> Cookings { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public enum RecipeType
    {
        Aperitif,
        Starter,
        Main,
        Dessert
    }    
}