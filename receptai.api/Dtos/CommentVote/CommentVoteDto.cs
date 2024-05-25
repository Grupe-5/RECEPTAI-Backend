using receptai.data;

namespace receptai.api.Dtos.CommentVote;

public class CommentVoteDto
{
    public int UserId { get; set; }

    public int CommentId { get; set; }

    public VoteType VoteType { get; set; }

    public DateTime VoteDate { get; set; }
}
