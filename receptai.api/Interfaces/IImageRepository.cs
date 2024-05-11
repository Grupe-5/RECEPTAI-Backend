namespace receptai.api;

public interface IImageRepository
{
    Task<int> UploadImageAsync(byte[] bytes);
    Task<byte[]?> RetrieveImageAsync(int imageId);
    Task<bool> DeleteImageAsync(int imageId);
}
