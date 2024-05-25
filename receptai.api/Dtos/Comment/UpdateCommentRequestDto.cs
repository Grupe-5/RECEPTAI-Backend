using System.ComponentModel.DataAnnotations;

namespace receptai.api.Dtos.Comment;

public class UpdateCommentRequestDto
{
    [Required]
    [MaxLength(1000, ErrorMessage = "Comment may not be longer than 1000 characters!")]
    public string CommentText { get; set; } = null!;

    [Required]
    public Guid Version { get; set; }
}
