using receptai.api.Dtos.Recipe;
using receptai.data;

namespace receptai.api.Mappers;

public static class RecipeMappers
{
    public static RecipeDto ToRecipeDto(this Recipe recipeModel)
    {
        return new RecipeDto
        {
            RecipeId = recipeModel.RecipeId,
            UserId = recipeModel.UserId,
            Title = recipeModel.Title,
            ImgId = recipeModel.ImgId,
            SubfoodditId = recipeModel.SubfoodditId,
            Ingredients = recipeModel.Ingredients,
            Description = recipeModel.Description,
            CookingTime = recipeModel.CookingTime,
            Servings = recipeModel.Servings,
            DatePosted = recipeModel.DatePosted,
            CookingDifficulty = recipeModel.CookingDifficulty,
            Instructions = recipeModel.Instructions
        };
    }

    public static Recipe ToRecipeFromCreateDto(
        this CreateRecipeRequestDto recipeModel)
    {
        return new Recipe
        {
            UserId = recipeModel.UserId,
            Title = recipeModel.Title,
            ImgId = recipeModel.ImgId,
            SubfoodditId = recipeModel.SubfoodditId,
            Ingredients = recipeModel.Ingredients,
            Description = recipeModel.Description,
            CookingTime = recipeModel.CookingTime,
            Servings = recipeModel.Servings,
            DatePosted = recipeModel.DatePosted,
            CookingDifficulty = recipeModel.CookingDifficulty,
            Instructions = recipeModel.Instructions
        };
    }
}