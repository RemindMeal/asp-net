using System.Collections.Immutable;
using System.Linq;
using AutoMapper;
using RemindMeal.Models;
using RemindMeal.ModelViews;
using RemindMeal.Pages.Friends;

namespace RemindMeal.Profiles
{
    public sealed class RemindMealProfile : Profile
    {
        public RemindMealProfile()
        {
            CreateMap<FriendView, Friend>()
                .ForMember(friend => friend.Id, opt => opt.Ignore())
                .ForMember(friend => friend.User, opt => opt.Ignore());

            CreateMap<RecipeModelView, Recipe>()
                .ForMember(recipe => recipe.User, opt => opt.Ignore())
                .ForMember(recipe => recipe.CreationDate, opt => opt.Ignore())
                .ForMember(recipe => recipe.RecipeTags, opt => opt.MapFrom(
                    recipeView => recipeView.SelectedTagIds.Select(tagId => new RecipeTag { RecipeId = recipeView.Id, TagId = tagId}).ToImmutableArray()));

            CreateMap<Recipe, RecipeModelView>()
                .ForMember(recipeView => recipeView.AvailableTags, opt => opt.Ignore())
                .ForMember(recipeView => recipeView.SelectedTagIds, opt => opt.MapFrom(
                    recipe => recipe.RecipeTags.Select(recipeTag => recipeTag.TagId).ToList()));

            CreateMap<MealModelView, Meal>()
                .ForMember(meal => meal.User, opt => opt.Ignore())
                .ForMember(meal => meal.Cookings, opt => opt.MapFrom(
                    mealView => mealView.SelectedRecipeIds.Select(recipeId => new Cooking { RecipeId = recipeId, MealId = mealView.Id }).ToImmutableArray()
                ))
                .ForMember(meal => meal.Presences, opt => opt.MapFrom(
                    mealView => mealView.SelectedFriendIds.Select(friendId => new Presence { FriendId = friendId, MealId = mealView.Id }).ToImmutableArray()
                ));

            CreateMap<Meal, MealModelView>()
                .ForMember(mealView => mealView.AvailableFriends, opt => opt.Ignore())
                .ForMember(mealView => mealView.AvailableRecipes, opt => opt.Ignore())
                .ForMember(mealView => mealView.SelectedFriendIds, opt => opt.MapFrom(meal => meal.Presences.Select(p => p.FriendId).ToImmutableArray()))
                .ForMember(mealView => mealView.SelectedRecipeIds, opt => opt.MapFrom(meal => meal.Cookings.Select(c => c.RecipeId).ToImmutableArray()))
                .ForMember(mealView => mealView.Friends, opt => opt.MapFrom(meal => meal.Presences.Select(p => p.Friend).ToImmutableArray()))
                .ForMember(mealView => mealView.Recipes, opt => opt.MapFrom(meal => meal.Cookings.Select(p => p.Recipe).ToImmutableArray()));
        }
    }
}