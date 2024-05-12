using receptai.data;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.api.Dtos.RecipeVote;

public class UpdateRecipeVoteRequestDto
{
    public VoteType VoteType { get; set; }

    public DateTime VoteDate { get; set; } = DateTime.Now;
}
