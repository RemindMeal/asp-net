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
    public ICollection<Presence> Presences { get; } = [];
    public ICollection<Cooking> Cookings { get; } = [];

    public override string ToString()
    {
        return $"Meal of {Date}";
    }
}