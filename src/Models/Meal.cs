using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Meal : IHasUser
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Relationships
        [Required]
        public User User { get; set; }
        public ICollection<Presence> Presences { get; } = new List<Presence>();
        public ICollection<Cooking> Cookings { get; } = new List<Cooking>();
    }
}