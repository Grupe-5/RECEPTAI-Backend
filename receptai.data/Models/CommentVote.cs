using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace receptai.data;

public class CommentVote
{
        [Column(Order = 0)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        [Required]
        public VoteType VoteType { get; set; }

        [Required]
        public DateTime VoteDate { get; set; }

        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
}
