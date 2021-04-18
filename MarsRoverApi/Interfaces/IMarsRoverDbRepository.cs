using MarsRoverApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRoverApi.Interfaces
{
    public interface IMarsRoverDbRepository
    {
        // Rovers
        Task<bool> DoRoversExists();
        Task<IList<Rover>> CreateRovers(IList<Rover> rovers);
        Task<IList<Rover>> GetRovers();
        Task<Rover> GetRover(int roverId);

        // Photos
        Task<MarsPhoto> CreateMarsPhoto(MarsPhoto marsPhoto);
        Task<int> GetMarsPhotosByRoverDateCount(int roverId, string earthDate);
        Task<IList<MarsPhoto>> GetMarsPhotos(int roverId, string earthDate);
        Task<PagedResult<MarsPhoto>> GetMarsPhotosPagedAsync(int roverId, string earthDate, int page, int pageSize);

        // Photo
        Task<bool> DoMarsPhotoExists(int nasaPhotoId);
        Task<IList<MarsPhoto>> GetMarsPhoto(int photoId);
    }
}
