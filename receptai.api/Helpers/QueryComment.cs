using receptai.api.Dtos.Comment;

namespace receptai.api.Helpers;

public class QueryComment
{
    public int? RecipeId { get; set; } = null;

    public int? UserId { get; set; } = null;
}
