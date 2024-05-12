using receptai.data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.api.Dtos.CommentVote;

public class CreateCommentVoteRequestDto
{
    [Required]
    [Column(Order = 1)]
    [ForeignKey("Comment")]
    public int CommentId { get; set; }

    [Required]
    public VoteType VoteType { get; set; }

    [Required]
    public DateTime VoteDate { get; set; } = DateTime.Now;
}
