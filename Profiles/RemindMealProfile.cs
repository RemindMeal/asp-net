using System.Collections.Immutable;
using System.Linq;
using AutoMapper;
using RemindMeal.Models;
using RemindMeal.ModelViews;

namespace RemindMeal.Profiles
{
    public sealed class RemindMealProfile : Profile
    {
        public RemindMealProfile()
        {
            CreateMap<FriendModelView, Friend>()
                .ForMember(friend => friend.Id, opt => opt.Ignore())
                .ForMember(friend => friend.User, opt => opt.Ignore());

            CreateMap<RecipeModelView, Recipe>()
                .ForMember(recipe => recipe.Id, opt => opt.Ignore())
                .ForMember(recipe => recipe.User, opt => opt.Ignore())
                .ForMember(recipe => recipe.CreationDate, opt => opt.Ignore());

            CreateMap<RecipeEditModelView, Recipe>()
                .ForMember(recipe => recipe.User, opt => opt.Ignore())
                .ForMember(recipe => recipe.CreationDate, opt => opt.Ignore());

            CreateMap<MealModelView, Meal>()
                .ForMember(meal => meal.User, opt => opt.Ignore())
                .ForMember(meal => meal.Cookings, opt => opt.MapFrom(
                    mealMV => mealMV.SelectedRecipeIds.Select(recipeId => new Cooking { RecipeId = recipeId, MealId = mealMV.Id }).ToImmutableArray()
                ))
                .ForMember(meal => meal.Presences, opt => opt.MapFrom(
                    mealMV => mealMV.SelectedFriendIds.Select(friendId => new Presence { FriendId = friendId, MealId = mealMV.Id }).ToImmutableArray()
                ));

            CreateMap<Meal, MealModelView>()
                .ForMember(mealMV => mealMV.AvailableFriends, opt => opt.Ignore())
                .ForMember(mealMV => mealMV.AvailableRecipes, opt => opt.Ignore())
                .ForMember(mealMV => mealMV.SelectedFriendIds, opt => opt.MapFrom(meal => meal.Presences.Select(p => p.FriendId).ToImmutableArray()))
                .ForMember(mealMV => mealMV.SelectedRecipeIds, opt => opt.MapFrom(meal => meal.Cookings.Select(c => c.RecipeId).ToImmutableArray()));
        }
    }
}