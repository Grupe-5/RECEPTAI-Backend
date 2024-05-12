using receptai.data;

namespace receptai.api.Dtos.RecipeVote;

public class UpdateRecipeVoteRequestDto
{
    public VoteType VoteType { get; set; }

    public DateTime VoteDate { get; set; } = DateTime.Now;
}
