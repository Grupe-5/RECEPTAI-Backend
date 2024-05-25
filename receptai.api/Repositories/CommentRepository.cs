using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Comment;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly RecipePlatformDbContext _context;

    public CommentRepository(RecipePlatformDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var commentModel = await _context.Comments
            .FirstOrDefaultAsync(c => c.CommentId == id);

        if (commentModel is null)
        {
            return null;
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<List<Comment>> GetCommentsByUserId(int userId)
    {
        return await _context.Comments
            .Where(c => c.UserId == userId)
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByRecipeId(int recipeId)
    {
        return await _context.Comments
            .Where(c => c.RecipeId == recipeId)
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task<Comment?> UpdateAsync(int id,
        UpdateCommentRequestDto commentDto)
    {
        var existingComment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CommentId == id);

        if (existingComment is null)
        {
            return null;
        }

        if (existingComment.Version == commentDto.Version)
        {
            existingComment.CommentText = commentDto.CommentText;
            existingComment.Version = Guid.NewGuid();
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new DbUpdateConcurrencyException(
                "A concurrency conflict while updating the comment has occured.");
        }

        return existingComment;
    }

    public async Task<int> RecalculateVotesAsync(int commentId)
    {
        var votes = await _context.CommentVotes
            .Where(cv => cv.CommentId == commentId)
            .ToListAsync();

        var aggregatedVotes = votes.Sum(v => v.VoteType == VoteType.Upvote ? 1 : -1);

        var comment = await _context.Comments.FindAsync(commentId);

        if (comment is not null)
        {
            comment.AggregatedVotes = aggregatedVotes;
            await _context.SaveChangesAsync();
        }

        return aggregatedVotes;
    }
}
