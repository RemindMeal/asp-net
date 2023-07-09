using System.ComponentModel.DataAnnotations;

namespace RemindMealData.Models;

public sealed class Category : IHasUser
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le groupe doit avoir un nom")]
    public string Name { get; set; }

    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public User User { get; set; }

    public override string ToString() => Name;
}