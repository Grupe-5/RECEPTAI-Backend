using receptai.data;

namespace receptai.api;

public class ImageRepository(RecipePlatformDbContext context) : IImageRepository
{
    private readonly RecipePlatformDbContext _context = context;

    public async Task<bool> DeleteImageAsync(int imageId)
    {
        var img = await _context.Images.FindAsync(imageId);
        if (img == null)
        {
            return false;
        }
        else
        {
            _context.Images.Remove(img);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<byte[]?> RetrieveImageAsync(int imageId)
    {
        var img = await _context.Images.FindAsync(imageId);
        return img?.ImageData;
    }

    public async Task<int> UploadImageAsync(byte[] bytes)
    {
        Image img = new()
        {
            ImgId = 0,
            ImageData = bytes,
        };
        var ent = await _context.Images.AddAsync(img);
        await _context.SaveChangesAsync();
        return ent.Entity.ImgId;
    }
}
