using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace receptai.data;

public class User : IdentityUser<int>
{
    [Required]
    public DateTime JoinDate { get; set; } = DateTime.Now;

    [Required]
    public int KarmaScore { get; set; }

    [ForeignKey("Image")]
    public int? ImgId { get; set; }

    public virtual Image? Image { get; set; }

    public virtual ICollection<Recipe>? Recipes { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }
    public virtual ICollection<RecipeVote>? RecipeVotes { get; set; }
    public virtual ICollection<CommentVote>? CommentVotes { get; set; }
    public virtual ICollection<Subfooddit> Subfooddits { get; set; } = new List<Subfooddit>();
}
