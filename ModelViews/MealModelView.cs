using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RemindMeal.Models;

namespace RemindMeal.ModelViews
{
    public sealed class MealModelView
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Invités")]
        public ICollection<Friend> Friends { get; set; }

        [Display(Name = "Menu")]
        public ICollection<Recipe> Recipes { get; set; }
    }
}