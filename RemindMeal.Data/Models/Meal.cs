using System.ComponentModel.DataAnnotations;

namespace RemindMealData.Models;

public sealed class Meal : IHasUser
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    // Relationships
    [Required]
    public User User { get; set; }
    public ICollection<Presence> Presences { get; } = new List<Presence>();
    public ICollection<Cooking> Cookings { get; } = new List<Cooking>();

    public override string ToString()
    {
        return $"Meal of {Date}";
    }
}