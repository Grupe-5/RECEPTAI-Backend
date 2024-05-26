using receptai.api.Dtos.RecipeVote;
using receptai.data;

namespace receptai.api.Interfaces;

public interface IRecipeVoteRepository
{
    Task<RecipeVote?> GetByUserAndRecipeId(int userId, int recipeId);

    Task<RecipeVote> CreateAsync(RecipeVote recipeVoteModel, int userId);

    Task<RecipeVote?> UpdateAsync(int userId, int recipeId,
        UpdateRecipeVoteRequestDto recipeVoteDto);

    Task<RecipeVote?> DeleteAsync(int userId, int recipeId);

    Task<int> GetAggregatedRecipeVotesByRecipeId(int recipeId);

    Task<List<RecipeVote>> GetByRecipeId(int recipeId);
}
