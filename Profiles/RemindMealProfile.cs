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
            
            CreateMap<MealModelView, Meal>()
                .ForMember(meal => meal.User, opt => opt.Ignore());
        }
    }
}