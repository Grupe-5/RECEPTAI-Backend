namespace receptai.api.Dtos.Recipe;

public class RecipeDto
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public int? ImgId { get; set; }

    public int SubfoodditId { get; set; }

    public string Ingredients { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public string? CookingTime { get; set; } = null!;

    public int Servings { get; set; }

    public DateTime DatePosted { get; set; }

    public int CookingDifficulty { get; set; }

    public string? Instructions { get; set; } = null!;
}
