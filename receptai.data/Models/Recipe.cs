using System;
using System.Collections.Generic;
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

    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [ForeignKey("Image")]
    public int? ImgId { get; set; }

    [Required]
    [ForeignKey("Subreddit")]
    public int SubredditId { get; set; }

    public string Ingredients { get; set; }

    [StringLength(5000)]
    public string Description { get; set; }

    public string CookingTime { get; set; }

    public int Servings { get; set; }

    public DateTime DatePosted { get; set; }

    public string CookingDifficulty { get; set; }

    public string Instructions { get; set; }

    public virtual User User { get; set; }
    public virtual Image? Image { get; set; }
    public virtual Subreddit Subreddit { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
}
