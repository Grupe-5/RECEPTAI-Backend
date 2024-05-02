using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace receptai.data;

public class User
{
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public int KarmaScore { get; set; }

        [ForeignKey("Image")]
        public int? ImgId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<RecipeVote> RecipeVotes { get; set; }
        public virtual ICollection<CommentVote> CommentVotes { get; set; }
        public virtual ICollection<Subreddit> Subreddits { get; set; }
}
