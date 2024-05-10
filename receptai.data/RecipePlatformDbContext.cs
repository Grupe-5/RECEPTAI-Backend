using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace receptai.data;

public class RecipePlatformDbContext(
    DbContextOptions<RecipePlatformDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Subfooddit> Subfooddits { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RecipeVote> RecipeVotes { get; set; }
    public DbSet<CommentVote> CommentVotes { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<RecipeVote>()
            .HasKey(v => new { v.UserId, v.RecipeId });
        modelBuilder.Entity<CommentVote>()
            .HasKey(v => new { v.UserId, v.CommentId });
        
        List<IdentityRole<int>> roles =
        [
            new IdentityRole<int>
            {
                Id = -1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<int>
            {
                Id = -2,
                Name = "User",
                NormalizedName = "USER"
            },
        ];
        modelBuilder.Entity<IdentityRole<int>>().HasData(roles);
    }
}
