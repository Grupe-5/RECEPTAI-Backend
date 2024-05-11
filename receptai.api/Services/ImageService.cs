using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace receptai.api;

public class ImageService(IImageRepository imageRepo) : IImageService
{
    private readonly IImageRepository _imageRepo = imageRepo;

    public async Task<bool> DeleteImageAsync(int imageId)
    {
        return await _imageRepo.DeleteImageAsync(imageId);
    }

    public async Task<byte[]?> FetchImageAsync(int imageId)
    {
        return await _imageRepo.RetrieveImageAsync(imageId);
    }

    public async Task<int> StoreImageAsync(Stream file, ImageDimensions? dimensions)
    {
        using var image = Image.Load(file);
        if (dimensions.HasValue)
        {
            var resizeOptions = new ResizeOptions
            {
                Mode = dimensions.Value.KeepAspectRatio ? ResizeMode.Max : ResizeMode.Stretch,
                Size = new Size(dimensions.Value.Width, dimensions.Value.Height)
            };
            image.Mutate(x => x.Resize(resizeOptions));
        }
        
        var jpegEncoder = new JpegEncoder
        {
            Quality = 75
        };

        /* Hella inefficient, but w/e (essentially 3-4 copies of image data) */
        using var outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, jpegEncoder);
        return await _imageRepo.UploadImageAsync(outputStream.ToArray());
    }
}
