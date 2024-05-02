using Microsoft.EntityFrameworkCore;

namespace receptai.data;

public class RecipePlatformDbContext(DbContextOptions<RecipePlatformDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subreddit> Subreddits { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RecipeVote> RecipeVotes { get; set; }
    public DbSet<CommentVote> CommentVotes { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeVote>()
            .HasKey(v => new { v.UserId, v.RecipeId });
        modelBuilder.Entity<CommentVote>()
            .HasKey(v => new { v.UserId, v.CommentId });
    }
}
