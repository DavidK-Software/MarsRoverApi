using MarsRoverApi.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public class ImageService: IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly IImageProviderSettings _imageProviderSettings;

        public ImageService(
            ILogger<ImageService> logger,
            IImageProviderSettings imageProviderSettings
            )
        {
            _logger = logger;
            _imageProviderSettings = imageProviderSettings;
        }

        public bool EnsureImagePathCreated(string imagePath)
        {
            bool success = false;
            if (!Directory.Exists(imagePath))
            {
                try
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(imagePath);
                    success = true;
                }
                catch (System.Exception ex)
                {
                    var message = $"Failed to create image path. ImagePath: {imagePath} Error: {ex.Message}";
                    _logger.LogError(message);
                    throw; // todo: replace with a domain exception class
                }
            }
            return success;
        }

        public async Task<byte[]> GetImageAsync(string imagePath, string imageFileName)
        {
            byte[] image = null;

            try
            {
                var imageFilePath = Path.Combine(imagePath, imageFileName);

                var fileInfo = _imageProviderSettings.FileProvider.GetFileInfo(imageFilePath);

                if (fileInfo != null && fileInfo.Exists)
                {
                    image = await File.ReadAllBytesAsync(imageFilePath);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to retrieve image file. ImagePath: {imagePath} FileName: {imageFileName} Error: {ex.Message}");
            }

            return image;
        }

        public async Task<string> SaveImageAsync(string imagePath, string imageFileName, byte[] image)
        {
            try
            {
                var imageFilePath = Path.Combine(imagePath, imageFileName);

                var fileInfo = _imageProviderSettings.FileProvider.GetFileInfo(imageFilePath);

                // Only save the image if it doesn't already exist
                if (!fileInfo.Exists)
                {
                    using (var file = File.Create(fileInfo.PhysicalPath))
                    {
                        await file.WriteAsync(image, 0, image.Length);
                    }
                }
                return imageFilePath;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to save image file. ImagePath: {imagePath} FileName: {imageFileName} Error: {ex.Message}");
            }

            return null;
        }
    }
}
