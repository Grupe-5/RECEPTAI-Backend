using receptai.api.Dtos.Recipe;
using receptai.data;

namespace receptai.api.Interfaces;

public enum RecipeSortEnum {
    ByPostDate,
    ByKarma
};

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync(int offset = 0, int limit = 50, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false);

    Task<List<Recipe>> GetJoinedAsync(User user, int offset = 0, int limit = 50, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false);

    Task<Recipe?> GetByIdAsync(int id);

    Task<Recipe> CreateAsync(Recipe recipeModel);

    Task<Recipe?> UpdateAsync(int id,
        UpdateRecipeRequestDto recipeDto, int? imageId, bool remove_photo);
    
    Task<Recipe?> DeleteAsync(int id);

    Task<int> RecalculateVotesAsync(int recipeId);

    Task<List<Recipe>> GetRecipesByUserId(int userId, int offset = 0, int limit = 50, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false);

    Task<List<Recipe>> GetRecipesBySubfoodditId(int subfoodditId, int offset = 0, int limit = 50, RecipeSortEnum sort = RecipeSortEnum.ByPostDate, bool asc = false);
}
