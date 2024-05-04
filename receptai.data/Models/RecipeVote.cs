using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class RecipeVote
{
    [Required]
    [Column(Order = 0)]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    [Column(Order = 1)]
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }

    [Required]
    public VoteType VoteType { get; set; }

    [Required]
    public DateTime VoteDate { get; set; } = DateTime.Now;

    public virtual User User { get; set; } = null!;
    public virtual Recipe Recipe { get; set; } = null!;
}
