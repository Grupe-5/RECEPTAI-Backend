using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Helpers;
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
            .FirstOrDefaultAsync(r => r.RecipeId == id);

        if (recipeModel is null)
        {
            return null;
        }

        _context.Recipes.Remove(recipeModel);
        await _context.SaveChangesAsync();

        return recipeModel;
    }

    public async Task<List<Recipe>> GetAllAsync()
    {
        return await _context.Recipes.ToListAsync();
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        return await _context.Recipes.FindAsync(id);
    }

    public async Task<List<Recipe>> GetRecipesByUserId(int userId)
    {
        return await _context.Recipes
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Recipe>> GetRecipesBySubfoodditId(int subfoodditId)
    {
        return await _context.Recipes
            .Where(r => r.SubfoodditId == subfoodditId)
            .ToListAsync();
    }

    public async Task<Recipe?> UpdateAsync(int id,
        UpdateRecipeRequestDto recipeDto)
    {
        var existingRecipe = await _context.Recipes
            .FirstOrDefaultAsync(x => x.RecipeId == id);

        if (existingRecipe is null)
        {
            return null;
        }

        existingRecipe.Title = recipeDto.Title;
        existingRecipe.ImgId = recipeDto.ImgId;
        existingRecipe.Ingredients = recipeDto.Ingredients;
        existingRecipe.CookingTime = recipeDto.CookingTime;
        existingRecipe.Servings = recipeDto.Servings;
        existingRecipe.DatePosted = recipeDto.DatePosted;
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
