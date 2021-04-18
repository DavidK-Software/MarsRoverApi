using MarsRoverApi.Interfaces;
using Microsoft.Extensions.FileProviders;

namespace MarsRoverApi.Infrastructure
{
    public class ImageProviderSettings: IImageProviderSettings
    {
        public IFileProvider FileProvider { get; set; }
    }
}
