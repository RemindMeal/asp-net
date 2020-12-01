using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Friend : IHasUser
    {
        public Friend()
        {
            Presences = new List<Presence>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Surname { get; set; }
        
        public string FullName => $"{Name} {Surname}";
        
        // Relationships
        public User User { get; set; }
        public ICollection<Presence> Presences { get; set; }

        public override string ToString()
        {
            return FullName;
        }

    }
}