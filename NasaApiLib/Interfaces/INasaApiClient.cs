using NasaApiLib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NasaApiLib.Interfaces
{
    public interface INasaApiClient
    {
        Task<NasaMarsRovers> GetRoversAsync();

        Task<IList<NasaMarsPhoto>> GetRoverPhotosAsync(string roverName, string earthDate, int page);

        Task<byte[]> GetRoverPhotoAsync(string imgSrc);

        Task<NasaMarsPhotos> TestGetRoverPhotosAsync(string roverName, string earthDate, int page);
    }
}
