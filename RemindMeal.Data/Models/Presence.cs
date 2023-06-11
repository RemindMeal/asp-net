namespace RemindMealData.Models;

public sealed class Presence
{
    public int FriendId { get; set; }
    public int MealId { get; set; }

    // Relationships
    public Friend Friend { get; set; }
    public Meal Meal { get; set; }

    public override string ToString()
    {
        return $"{Friend} at {Meal.Date}";
    }
}
