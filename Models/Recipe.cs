using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Recipe : IHasUser
    {
        public Recipe()
        {
            CreationDate = DateTime.Now;
            Cookings = new List<Cooking>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Date de création")]
        public DateTime CreationDate { get; }
        
        [Required]
        public RecipeType Type { get; set; }
        
        // Relationships
        [Required]
        public User User { get; set; }
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