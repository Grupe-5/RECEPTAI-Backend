using receptai.api.Dtos.Recipe;
using receptai.data;

namespace receptai.api.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync();

    Task<Recipe?> GetByIdAsync(int id);

    Task<Recipe> CreateAsync(Recipe recipeModel);

    Task<Recipe?> UpdateAsync(int id,
        UpdateRecipeRequestDto recipeDto);
    
    Task<Recipe?> DeleteAsync(int id);

    Task<int> RecalculateVotesAsync(int recipeId);

    Task<List<Recipe>> GetRecipesByUserId(int userId);

    Task<List<Recipe>> GetRecipesBySubfoodditId(int subfoodditId);
}
