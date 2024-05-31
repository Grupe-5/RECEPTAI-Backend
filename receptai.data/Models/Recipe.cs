using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public int AggregatedVotes { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [ForeignKey("Image")]
    public int? ImgId { get; set; }

    [Required]
    [ForeignKey("Subfooddit")]
    public int SubfoodditId { get; set; }

    public string SubfoodditName { get; set; } = null!;

    [Required]
    public string Ingredients { get; set; } = null!;

    public string? CookingTime { get; set; }

    [Range(1, int.MaxValue)]
    public int Servings { get; set; }

    [Required]
    public DateTime DatePosted { get; set; } = DateTime.Now;

    [Range(1, 10)]
    public int CookingDifficulty { get; set; }

    [Required]
    public string? Instructions { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Image? Image { get; set; }
    public virtual Subfooddit Subfooddit { get; set; } = null!;
    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    public virtual ICollection<RecipeVote>? Votes { get; set; } = new List<RecipeVote>();
}
