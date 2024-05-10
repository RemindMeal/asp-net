using Microsoft.EntityFrameworkCore;
using RemindMealData.Models;

namespace RemindMealData;

public interface IDbSetProvider< TModel > where TModel : class
{
    public DbSet<TModel> GetDbSet();
}

internal class DbSetProviderFromDbContext<TModel>(IDbContextFactory<RemindMealContext> contextFactory, Func<RemindMealContext, DbSet<TModel>> selector) :
    IDbSetProvider<TModel>, IAsyncDisposable where TModel : class
{
    private readonly RemindMealContext context = contextFactory.CreateDbContext();

    public async ValueTask DisposeAsync() => await context.DisposeAsync();

    public DbSet<TModel> GetDbSet() => selector(context);
}

internal class FriendDbSetProvider(IDbContextFactory<RemindMealContext> contextFactory) :
    DbSetProviderFromDbContext<Friend>(contextFactory, context => context.Friends)
{
}

internal class RecipeDbSetProvider(IDbContextFactory<RemindMealContext> contextFactory) :
    DbSetProviderFromDbContext<Recipe>(contextFactory, context => context.Recipes)
{
}