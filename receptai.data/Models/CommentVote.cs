using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class CommentVote
{
    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("Comment")]
    public int CommentId { get; set; }

    [Required]
    public VoteType VoteType { get; set; }

    [Required]
    public DateTime VoteDate { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
    public virtual Comment Comment { get; set; } = null!;
}
