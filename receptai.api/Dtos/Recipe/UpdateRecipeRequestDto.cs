using System.ComponentModel.DataAnnotations;

namespace receptai.api.Dtos.Recipe;

public class UpdateRecipeRequestDto
{
    [Required]
    [MaxLength(255, ErrorMessage = "Title may not be longer than 255 characters!")]
    public string Title { get; set; } = null!;

    public int? ImgId { get; set; }

    [Required]
    public string Ingredients { get; set; } = null!;

    [MaxLength(5000, ErrorMessage = "Description may not be longer than 5000 characters!")]
    public string? Description { get; set; } = null!;

    public string? CookingTime { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Servings must be a positive number!")]
    public int Servings { get; set; }

    [Required]
    public DateTime DatePosted { get; set; }

    [Range(1, 10, ErrorMessage = "Difficulty must be rated between 1 and 10!")]
    public int CookingDifficulty { get; set; }

    [Required]
    public string? Instructions { get; set; } = null!;
}
