using System.ComponentModel.DataAnnotations;

namespace RemindMealData.Models;

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
    [Display(Name = "Nom")]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    [Display(Name = "Date de création")]
    [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
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
    [Display(Name = "Apéritif")]
    Aperitif,
    [Display(Name = "Entrée")]
    Starter,
    [Display(Name = "Plat principal")]
    Main,
    [Display(Name = "Soupe")]
    Soup,
    [Display(Name = "Viande")]
    Meat,
    [Display(Name = "Légumes")]
    Side,
    [Display(Name = "Dessert")]
    Dessert
}
