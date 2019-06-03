using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RemindMeal.Models
{
    public sealed class Meal : IHasUser
    {
        public Meal()
        {
            Presences = new List<Presence>();
            Cookings = new List<Cooking>();
        }
        
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Relationships
        [Required]
        public User User { get; set; }
        public ICollection<Presence> Presences { get; }
        public ICollection<Cooking> Cookings { get; }

        [NotMapped]
        public ICollection<Friend> Friends => Presences.Select(p => p.Friend).ToImmutableArray();

        [NotMapped]
        public ICollection<Recipe> Recipes => Cookings.Select(c => c.Recipe).ToImmutableArray();
    }
}