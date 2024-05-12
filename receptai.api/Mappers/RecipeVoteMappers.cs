using receptai.api.Dtos.RecipeVote;
using receptai.data;

namespace receptai.api.Mappers;

public static class RecipeVoteMappers
{
    public static RecipeVoteDto ToRecipeVoteDto(this RecipeVote recipeVoteModel)
    {
        return new RecipeVoteDto
        {
            UserId = recipeVoteModel.UserId,
            RecipeId = recipeVoteModel.RecipeId,
            VoteType = recipeVoteModel.VoteType,
            VoteDate = recipeVoteModel.VoteDate
        };
    }

    public static RecipeVote ToRecipeVoteFromCreateDto(
        this CreateRecipeVoteRequestDto recipeVoteModel)
    {
        return new RecipeVote
        {
            UserId = recipeVoteModel.UserId,
            RecipeId = recipeVoteModel.RecipeId,
            VoteType = recipeVoteModel.VoteType,
            VoteDate = recipeVoteModel.VoteDate
        };
    }

}
