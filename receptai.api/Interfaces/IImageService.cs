namespace receptai.api;

public record struct ImageDimensions
{
    public ImageDimensions() {}

    /* If both are zero then image is not resized */
    /* If one is zero, then image is resized to dimension, but keeping aspect ratio */
    /* If both are non-zero, then image is resized to those dimensions (or less, if KeepAspectRatio specified) */
    public int Width = 0;
    public int Height = 0;
    public bool KeepAspectRatio = false;
}

public interface IImageService
{
    Task<int> StoreImageAsync(Stream file, ImageDimensions? dimensions);
    Task<byte[]?> FetchImageAsync(int imageId);
    Task<bool> DeleteImageAsync(int imageId);
}
