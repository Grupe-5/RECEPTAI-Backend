using receptai.data;

namespace receptai.api.Dtos.RecipeVote;

public class RecipeVoteDto
{
    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public VoteType VoteType { get; set; }

    public DateTime VoteDate { get; set; }
}
