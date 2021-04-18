using MarsRoverApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRoverApi.Interfaces
{
    public interface IMarsPhotoRetrievalService
    {
        bool EnsureImagePathCreated();

        Task<IList<Rover>> InitializeRovers();

        Task<int> SaveRoverMarsPhotosAsync(Rover rover, string earthDate);

        Task<int> SaveAllRoverMarsPhotosAsync(string earthDate);
    }
}
