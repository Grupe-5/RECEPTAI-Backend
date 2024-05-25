using System.ComponentModel.DataAnnotations;
using receptai.data;

namespace receptai.api.Dtos.CommentVote;

public class UpdateCommentVoteRequestDto
{
    [Required]
    public VoteType VoteType { get; set; }
}
