using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipePlatformDbContext _context;

    public RecipeRepository(RecipePlatformDbContext context)
    {
        _context = context;
    }

    public async Task<Recipe> CreateAsync(Recipe recipeModel)
    {
        await _context.Recipes.AddAsync(recipeModel);
        await _context.SaveChangesAsync();

        return recipeModel;
    }

    public async Task<Recipe?> DeleteAsync(int id)
    {
        var recipeModel = await _context.Recipes
            .Include(i => i.User)
            .FirstOrDefaultAsync(r => r.RecipeId == id);

        if (recipeModel is null)
        {
            return null;
        }

        _context.Recipes.Remove(recipeModel);
        await _context.SaveChangesAsync();

        return recipeModel;
    }

    private IOrderedQueryable<Recipe> getRecipeSort(RecipeSortEnum sort, bool asc)
    {
        switch (sort) {
            case RecipeSortEnum.ByPostDate:
                if (asc) {
                    return _context.Recipes.OrderBy(r => r.DatePosted);
                } else {
                    return _context.Recipes.OrderByDescending(r => r.DatePosted);
                }
            case RecipeSortEnum.ByKarma:
                if (asc) {
                    return _context.Recipes.OrderBy(r => r.AggregatedVotes);
                } else {
                    return _context.Recipes.OrderByDescending(r => r.AggregatedVotes);
                }
            default:
                throw new ArgumentException("Invalid sort");
        }
    }

    public async Task<List<Recipe>> GetAllAsync(int offset = 0, int limit = 10, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false)
    {
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset should be greater than or equal to 0.");
        if (limit < 1) throw new ArgumentOutOfRangeException(nameof(limit), "Limit should be greater than or equal to 1.");

        return await getRecipeSort(sort, asc)
            .Include(i => i.User)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Recipe>> GetJoinedAsync(User user, int offset = 0, int limit = 50, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false)
    {
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset should be greater than or equal to 0.");
        if (limit < 1) throw new ArgumentOutOfRangeException(nameof(limit), "Limit should be greater than or equal to 1.");

        return await getRecipeSort(sort, asc)
            .Include(i => i.User)
            .Include(i => i.Subfooddit.Users)
            .Where(i => i.Subfooddit.Users.Contains(user))
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        return await _context.Recipes
            .Include(i => i.User).FirstOrDefaultAsync(i => i.RecipeId == id);
    }

    public async Task<List<Recipe>> GetRecipesByUserId(int userId, int offset = 0, int limit = 10, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false)
    {
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset should be greater than or equal to 0.");
        if (limit < 1) throw new ArgumentOutOfRangeException(nameof(limit), "Limit should be greater than or equal to 1.");

        return await getRecipeSort(sort, asc)
            .Include(i => i.User)
            .Where(r => r.UserId == userId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Recipe>> GetRecipesBySubfoodditId(int subfoodditId, int offset = 0, int limit = 10, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false)
    {
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset should be greater than or equal to 0.");
        if (limit < 1) throw new ArgumentOutOfRangeException(nameof(limit), "Limit should be greater than or equal to 1.");

        return await getRecipeSort(sort, asc) 
            .Include(i => i.User)
            .Where(r => r.SubfoodditId == subfoodditId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Recipe?> UpdateAsync(int id, UpdateRecipeRequestDto recipeDto, int? imageId, bool remove_image)
    {
        var existingRecipe = await _context.Recipes
            .Include(i => i.User)
            .FirstOrDefaultAsync(x => x.RecipeId == id);
        if (existingRecipe is null)
        {
            return null;
        }

        if (imageId != null || remove_image) {
            if (existingRecipe.ImgId != null) {
                var existing_img = await _context.Images.FindAsync(existingRecipe.ImgId.Value);
                existingRecipe.ImgId = imageId;
                if (existing_img != null) {
                    _context.Images.Remove(existing_img);
                }
            }
            else
            {
                existingRecipe.ImgId = imageId;
            }
        }
        
        existingRecipe.Title = recipeDto.Title;
        existingRecipe.Ingredients = recipeDto.Ingredients;
        existingRecipe.CookingTime = recipeDto.CookingTime;
        existingRecipe.Servings = recipeDto.Servings;
        existingRecipe.CookingDifficulty = recipeDto.CookingDifficulty;
        existingRecipe.Instructions = recipeDto.Instructions;

        await _context.SaveChangesAsync();

        return existingRecipe;
    }

    public async Task<int> RecalculateVotesAsync(int recipeId)
    {
        var votes = await _context.RecipeVotes
            .Where(rv => rv.RecipeId == recipeId)
            .ToListAsync();

        var aggregatedVotes = votes.Sum(v => v.VoteType == VoteType.Upvote ? 1 : -1);

        var recipe = await _context.Recipes.FindAsync(recipeId);

        if (recipe is not null)
        {
            recipe.AggregatedVotes = aggregatedVotes;
            await _context.SaveChangesAsync();
        }

        return aggregatedVotes;
    }
}
