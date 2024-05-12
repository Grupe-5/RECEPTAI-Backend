using System.ComponentModel.DataAnnotations;

namespace receptai.api.Dtos.Comment;

public class CreateCommentRequestDto
{
    [Required]
    public int RecipeId { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Comment may not be longer than 1000 characters!")]
    public string CommentText { get; set; } = null!;

    [Required]
    public DateTime CommentDate { get; set; }
}