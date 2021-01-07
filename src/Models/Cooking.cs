namespace RemindMeal.Models
{
    public sealed class Cooking
    {
        public int MealId { get; set; }
        public int RecipeId { get; set; }
        public int Order { get; set; }
        
        // Relationships
        public Meal Meal { get; set; }
        public Recipe Recipe { get; set; }
    }
}