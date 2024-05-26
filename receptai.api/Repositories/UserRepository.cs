using Microsoft.EntityFrameworkCore;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RecipePlatformDbContext _context;

    public UserRepository(RecipePlatformDbContext context)
    {
        _context = context;
    }

    public async Task<int> RecalculateKarmaScoreAsync(int userId)
    {
        var karmaFromRecipes = await _context.Recipes
            .Where(r => r.UserId == userId)
            .SumAsync(r => r.AggregatedVotes);

        var karmaFromComments = await _context.Comments
            .Where(c => c.UserId == userId)
            .SumAsync(c => c.AggregatedVotes);

        var karmaScore = karmaFromRecipes + karmaFromComments;

        var user = await _context.Users.FindAsync(userId);

        if (user is not null)
        {
            user.KarmaScore = karmaScore;
            await _context.SaveChangesAsync();
        }

        return karmaScore;
    }
}
