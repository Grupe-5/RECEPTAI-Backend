using Microsoft.Extensions.Caching.Memory;
using receptai.data;

namespace receptai.api;

public class CachedImageRepository(IImageRepository imageRepository, IMemoryCache memoryCache) : IImageRepository
{
    private readonly IImageRepository _imageRepository = imageRepository;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<bool> DeleteImageAsync(int imageId)
    {
        bool deleted = await _imageRepository.DeleteImageAsync(imageId);
        if (deleted) {
            _memoryCache.Remove(imageId);
        }
        return deleted;
    }

    public async Task<byte[]?> RetrieveImageAsync(int imageId)
    {
        if (!_memoryCache.TryGetValue(imageId, out byte[]? image)) {
            image = await _imageRepository.RetrieveImageAsync(imageId);
            if (image == null) {
                return null;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));
            
            _memoryCache.Set(imageId, image, cacheEntryOptions);
        }

        return image;
    }

    public async Task<int> UploadImageAsync(byte[] bytes)
        => await _imageRepository.UploadImageAsync(bytes);
}