namespace receptai.api.Dtos.Comment;

public class CommentDto
{
    public int CommentId { get; set; }

    public int RecipeId { get; set; }

    public string UserName { get; set; } = null!;
    public int UserId { get; set; }

    public int AggregatedVotes { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime CommentDate { get; set; }
}
