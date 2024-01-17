using System.ComponentModel.DataAnnotations;

namespace RemindMealData.Models;

public sealed class Friend : IHasUser
{
    public Friend()
    {}

    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Prénom")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string Surname { get; set; }

    [Display(Name = "Nom")]
    public string FullName => $"{Name} {Surname}";

    // Relationships
    public User User { get; set; }
    public ICollection<Presence> Presences { get; set; } = new List<Presence>();

    public override string ToString()
    {
        return FullName;
    }

}
