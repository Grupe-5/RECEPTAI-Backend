using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class RecipeVote
{
        [Column(Order = 0)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        [Required]
        public VoteType VoteType { get; set; }

        [Required]
        public DateTime VoteDate { get; set; }

        public virtual User User { get; set; }
        public virtual Recipe Recipe { get; set; }
}
