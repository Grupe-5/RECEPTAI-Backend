using receptai.data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.api.Dtos.CommentVote;

public class CreateCommentVoteRequestDto
{
    [Required]
    [ForeignKey("Comment")]
    public int CommentId { get; set; }

    [Required]
    public VoteType VoteType { get; set; }
}
