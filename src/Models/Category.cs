using System.ComponentModel.DataAnnotations;

namespace RemindMeal.Models
{
    public sealed class Category : IHasUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Required]
        public User User { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}