using receptai.api.Dtos.CommentVote;
using receptai.data;

namespace receptai.api.Mappers;

public static class CommentVoteMappers
{
    public static CommentVoteDto ToCommentVoteDto(this CommentVote commentVoteModel)
    {
        return new CommentVoteDto
        {
            UserId = commentVoteModel.UserId,
            CommentId = commentVoteModel.CommentId,
            VoteType = commentVoteModel.VoteType,
            VoteDate = commentVoteModel.VoteDate
        };
    }

    public static CommentVote ToCommentVoteFromCreateDto(
        this CreateCommentVoteRequestDto commentVoteModel)
    {
        return new CommentVote
        {
            CommentId = commentVoteModel.CommentId,
            VoteType = commentVoteModel.VoteType,
            VoteDate = commentVoteModel.VoteDate
        };
    }

}
