using System;
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

    [Required]
    [StringLength(5000)]
    public string CommentText { get; set; }

    [Required]
    public DateTime CommentDate { get; set; }

    public virtual Recipe Recipe { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<CommentVote> Votes { get; set; }
}
