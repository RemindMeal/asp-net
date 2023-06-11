using Microsoft.AspNetCore.Identity;

namespace RemindMeal.Services
{
    public sealed class RemindMealIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Cette adresse mail existe déjà"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Ce nom d'utilisateur est déjà pris"
            };
        }
    }
}