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

}
