using receptai.api.Dtos.CommentVote;
using receptai.data;

namespace receptai.api.Interfaces;

public interface ICommentVoteRepository
{
    Task<CommentVote?> GetByUserAndCommentId(int userId, int commentId);

    Task<CommentVote> CreateAsync(CommentVote recipeVoteModel, int userId);

    Task<CommentVote?> UpdateAsync(int userId, int commentId,
        UpdateCommentVoteRequestDto commentVoteDto);

    Task<CommentVote?> DeleteAsync(int userId, int commentId);

    Task<int> GetAggregatedCommentVotesByCommentId(int commentId);

    Task<List<CommentVote>> GetByCommentId(int commentId);
}
