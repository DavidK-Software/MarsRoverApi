using Microsoft.Extensions.FileProviders;

namespace MarsRoverApi.Interfaces
{
    public interface IImageProviderSettings
    {
        public IFileProvider FileProvider { get; set; }
    }
}
