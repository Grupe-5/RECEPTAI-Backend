using receptai.api.Dtos.Recipe;
using receptai.api.Helpers;
using receptai.data;

namespace receptai.api.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync(
        QueryRecipe query);

    Task<Recipe?> GetByIdAsync(int id);

    Task<Recipe> CreateAsync(Recipe recipeModel);

    Task<Recipe?> UpdateAsync(int id,
        UpdateRecipeRequestDto recipeDto);
    
    Task<Recipe?> DeleteAsync(int id);

}
