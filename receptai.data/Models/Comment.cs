using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public int AggregatedVotes { get; set; }

    [Required]
    [StringLength(1000)]
    public string CommentText { get; set; } = null!;

    [Required]
    public DateTime CommentDate { get; set; } = DateTime.Now;

    public virtual Recipe Recipe { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CommentVote>? Votes { get; set; }

    public Guid Version { get; set; }
}
