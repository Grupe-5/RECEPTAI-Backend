using System.ComponentModel.DataAnnotations;

namespace receptai.data;

public class Subfooddit
{
    [Key]
    public int SubfoodditId { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public virtual ICollection<Recipe>? Recipes { get; set; }
    public virtual ICollection<User>? Users { get; set; }
}
