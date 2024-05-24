using receptai.data;

namespace receptai.api.Dtos.Recipe;

public class RecipeDto
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int AggregatedVotes { get; set; }

    public int? ImgId { get; set; }

    public int SubfoodditId { get; set; }

    public string SubfoodditName { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string? CookingTime { get; set; } = null!;

    public int Servings { get; set; }

    public DateTime DatePosted { get; set; }

    public int CookingDifficulty { get; set; }

    public string? Instructions { get; set; } = null!;

    // Non-null if user logged in & voted for this recipe
    public VoteType? Vote { get; set; } = null;
}
