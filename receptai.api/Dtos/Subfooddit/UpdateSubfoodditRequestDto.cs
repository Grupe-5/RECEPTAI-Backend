using System.ComponentModel.DataAnnotations;


namespace receptai.api.Dtos.Subfooddit;

public class UpdateSubfoodditRequestDto
{
    [Required]
    [MaxLength(255, ErrorMessage = "Title may not be longer than 255 characters!")]
    public string Title { get; set; } = null!;


    [MaxLength(5000, ErrorMessage = "Description may not be longer than 5000 characters!")]
    public string? Description { get; set; }

}
