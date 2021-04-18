using System.Threading.Tasks;

namespace MarsRoverApi.Interfaces
{
    public interface IImageService
    {
        bool EnsureImagePathCreated(string imagePath);
        Task<byte[]> GetImageAsync(string imagePath, string imageFileName);
        Task<string> SaveImageAsync(string imagePath, string imageFileName, byte[] image);
    }
}
