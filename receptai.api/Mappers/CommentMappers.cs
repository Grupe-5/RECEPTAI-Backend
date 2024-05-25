using receptai.api.Dtos.Comment;
using receptai.data;

namespace receptai.api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel, VoteType? vote = null)
    {
        return new CommentDto
        {
            CommentId = commentModel.CommentId,
            RecipeId = commentModel.RecipeId,
            UserName = commentModel.UserName,
            UserId = commentModel.UserId,
            AggregatedVotes = commentModel.AggregatedVotes,
            CommentText = commentModel.CommentText,
            CommentDate = commentModel.CommentDate,
            Vote = vote,
            Version = commentModel.Version
        };
    }

    public static Comment ToCommentFromCreateDto(
        this CreateCommentRequestDto commentModel)
    {
        return new Comment
        {
            RecipeId = commentModel.RecipeId,
            CommentText = commentModel.CommentText,
            CommentDate = DateTime.UtcNow,
            Version = Guid.NewGuid()
        };
    }
}