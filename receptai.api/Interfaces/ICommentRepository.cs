using receptai.api.Dtos.Comment;
using receptai.api.Helpers;
using receptai.data;

namespace receptai.api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsByUserId(int userId);

    Task<List<Comment>> GetCommentsByRecipeId(int recipeId);

    Task<Comment?> GetByIdAsync(int id);

    Task<Comment> CreateAsync(Comment commentModel);

    Task<Comment?> UpdateAsync(int id,
        UpdateCommentRequestDto commentDto);

    Task<Comment?> DeleteAsync(int id);

    Task<int> RecalculateVotesAsync(int commentId);

}
