using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RemindMeal.Models;

namespace RemindMeal.ModelViews
{
	public sealed class MealModelView
	{
		public int Id { get; set; }

		[DataType(DataType.Date)]
		public DateTime Date { get; set; } = DateTime.Today;

		public ICollection<int> SelectedFriendIds { get; set; } = new List<int>();
		public ICollection<int> SelectedRecipeIds { get; set; } = new List<int>();

		public SelectList AvailableFriends { get; set; }
		public SelectList AvailableRecipes { get; set; }

		[Display(Name = "Invités")]
		public ICollection<Friend> Friends { get; set; }

		[Display(Name = "Menu")]
		public ICollection<Recipe> Recipes { get; set; }
	}
}