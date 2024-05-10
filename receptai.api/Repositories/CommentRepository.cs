using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Comment;
using receptai.api.Helpers;
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

    public async Task<List<Comment>> GetAllAsync(QueryComment query)
    {
        var comments = _context.Comments.AsQueryable();

        // Filter by RecipeId if provided
        if (query.RecipeId.HasValue)
        {
            comments = comments.Where(cr => cr.RecipeId == query.RecipeId.Value);
        }

        // Filter by UserId if provided
        if (query.UserId.HasValue)
        {
            comments = comments.Where(cu => cu.UserId == query.UserId.Value);
        }

        return await comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment?> UpdateAsync(int id,
        UpdateCommentRequestDto commentDto)
    {
        var existingComment = await _context.Comments
            .FirstOrDefaultAsync(xc => xc.CommentId == id);

        if (existingComment is null)
        {
            return null;
        }

        existingComment.CommentText = commentDto.CommentText;
        existingComment.CommentDate = commentDto.CommentDate;

        await _context.SaveChangesAsync();

        return existingComment;
    }
}
