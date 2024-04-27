using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace receptai.data;

public class Subreddit
{
        [Key]
        public int SubredditId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<User> Users { get; set; }
}
