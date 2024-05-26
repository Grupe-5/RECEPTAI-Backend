using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.RecipeVote;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class RecipeVoteRepository : IRecipeVoteRepository
{
    private readonly RecipePlatformDbContext _context;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;

    public RecipeVoteRepository(RecipePlatformDbContext context,
        IRecipeRepository recipeRepository, IUserRepository userRepository)
    {
        _context = context;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
    }

    public async Task<RecipeVote?> GetByUserAndRecipeId(int userId, int recipeId)
    {
        var recipeVoteModel = await _context.RecipeVotes
           .FirstOrDefaultAsync(rv => rv.UserId == userId &&
               rv.RecipeId == recipeId);

        return recipeVoteModel;
    }

    public async Task<List<RecipeVote>> GetByRecipeId(int recipeId)
    {
        var recipeVoteModels = await _context.RecipeVotes
           .Where(rv => rv.RecipeId == recipeId)
           .ToListAsync();

        return recipeVoteModels;
    }

    public async Task<RecipeVote> CreateAsync(RecipeVote recipeVoteModel)
    {
        await _context.RecipeVotes.AddAsync(recipeVoteModel);
        await _context.SaveChangesAsync();

        await _recipeRepository.RecalculateVotesAsync(recipeVoteModel.RecipeId);
        await _userRepository.RecalculateKarmaScoreAsync(recipeVoteModel.UserId);

        return recipeVoteModel;
    }

    public async Task<RecipeVote?> DeleteAsync(int userId, int recipeId)
    {
        var recipeVoteModel = await _context.RecipeVotes
            .FirstOrDefaultAsync(rv => rv.UserId == userId &&
                rv.RecipeId == recipeId);

        if (recipeVoteModel is null)
        {
            return null;
        }

        _context.RecipeVotes.Remove(recipeVoteModel);
        await _context.SaveChangesAsync();

        await _recipeRepository.RecalculateVotesAsync(recipeId);
        await _userRepository.RecalculateKarmaScoreAsync(userId);

        return recipeVoteModel;
    }

    public async Task<int> GetAggregatedRecipeVotesByRecipeId(int recipeId)
    {
        return await _context.RecipeVotes
            .Where(rv => rv.RecipeId == recipeId)
            .Select(rv => rv.VoteType == VoteType.Upvote ? 1 : -1)
            .SumAsync();
    }

    public async Task<RecipeVote?> UpdateAsync(int userId, int recipeId,
        UpdateRecipeVoteRequestDto recipeVoteDto)
    {
        var existingRecipeVote = await _context.RecipeVotes
            .FirstOrDefaultAsync(rv => rv.UserId == userId &&
                rv.RecipeId == recipeId);

        if (existingRecipeVote is null)
        {
            return null;
        }

        existingRecipeVote.VoteType = recipeVoteDto.VoteType;
        existingRecipeVote.VoteDate = DateTime.Now;

        await _context.SaveChangesAsync();
        await _recipeRepository.RecalculateVotesAsync(recipeId);
        await _userRepository.RecalculateKarmaScoreAsync(userId);

        return existingRecipeVote;
    }
}
