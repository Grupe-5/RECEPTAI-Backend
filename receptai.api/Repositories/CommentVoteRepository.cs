using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.CommentVote;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class CommentVoteRepository : ICommentVoteRepository
{
    private readonly RecipePlatformDbContext _context;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentVoteRepository(RecipePlatformDbContext context,
        ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _context = context;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task<CommentVote?> GetByUserAndCommentId(int userId, int commentId)
    {
        var commentVoteModel = await _context.CommentVotes
           .FirstOrDefaultAsync(cv => cv.UserId == userId &&
               cv.CommentId == commentId);

        return commentVoteModel;
    }

    public async Task<List<CommentVote>> GetByCommentId(int commentId)
    {
        var commentVoteModels = await _context.CommentVotes
           .Where(cv => cv.CommentId == commentId)
           .ToListAsync();

        return commentVoteModels;
    }

    public async Task<CommentVote> CreateAsync(CommentVote commentVoteModel)
    {
        await _context.CommentVotes.AddAsync(commentVoteModel);
        await _context.SaveChangesAsync();

        await _commentRepository.RecalculateVotesAsync(commentVoteModel.CommentId);
        await _userRepository.RecalculateKarmaScoreAsync(commentVoteModel.UserId);

        return commentVoteModel;
    }

    public async Task<CommentVote?> DeleteAsync(int userId, int commentId)
    {
        var commentVoteModel = await _context.CommentVotes
            .FirstOrDefaultAsync(cv => cv.UserId == userId &&
                cv.CommentId == commentId);

        if (commentVoteModel is null)
        {
            return null;
        }

        _context.CommentVotes.Remove(commentVoteModel);
        await _context.SaveChangesAsync();

        await _commentRepository.RecalculateVotesAsync(commentId);
        await _userRepository.RecalculateKarmaScoreAsync(userId);

        return commentVoteModel;
    }

    public async Task<int> GetAggregatedCommentVotesByCommentId(int commentId)
    {
        return await _context.CommentVotes
            .Where(cv => cv.CommentId == commentId)
            .Select(cv => cv.VoteType == VoteType.Upvote ? 1 : -1)
            .SumAsync();
    }

    public async Task<CommentVote?> UpdateAsync(int userId, int commentId,
        UpdateCommentVoteRequestDto commentVoteDto)
    {
        var existingCommentVote = await _context.CommentVotes
            .FirstOrDefaultAsync(cv => cv.UserId == userId &&
                cv.CommentId == commentId);

        if (existingCommentVote is null)
        {
            return null;
        }

        existingCommentVote.VoteType = commentVoteDto.VoteType;
        existingCommentVote.VoteDate = DateTime.Now;

        await _context.SaveChangesAsync();
        await _commentRepository.RecalculateVotesAsync(commentId);
        await _userRepository.RecalculateKarmaScoreAsync(userId);

        return existingCommentVote;
    }
}
