using receptai.data;

namespace receptai.api.Dtos.CommentVote;

public class UpdateCommentVoteRequestDto
{
    public VoteType VoteType { get; set; }

    public DateTime VoteDate { get; set; } = DateTime.Now;
}
