using System.ComponentModel.DataAnnotations;

namespace receptai.api.Dtos.Recipe;

public class UpdateRecipeRequestDto
{
    [Required]
    [MaxLength(255, ErrorMessage = "Title may not be longer than 255 characters!")]
    public string Title { get; set; } = null!;

    [Required]
    public string Ingredients { get; set; } = null!;

    public IFormFile? Photo { get; set; }

    public string? CookingTime { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Servings must be a positive number!")]
    public int Servings { get; set; }

    [Range(1, 10, ErrorMessage = "Difficulty must be rated between 1 and 10!")]
    public int CookingDifficulty { get; set; }

    [Required]
    public string? Instructions { get; set; } = null!;
}
