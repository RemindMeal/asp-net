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
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        // Relationships
        [Required]
        public User User { get; set; }
        public ICollection<Presence> Presences { get; } = new List<Presence>();
        public ICollection<Cooking> Cookings { get; } = new List<Cooking>();
    }
}