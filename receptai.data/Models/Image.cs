using System.ComponentModel.DataAnnotations;

namespace receptai.data;

/* Maybe consider a KVP store instead? DB seems overkill */
public class Image
{
    [Key]
    public int ImgId { get; set; }

    [Required]
    public byte[] ImageData { get; set; } = null!;
}
