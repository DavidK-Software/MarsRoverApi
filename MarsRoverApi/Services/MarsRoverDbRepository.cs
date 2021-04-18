using MarsRoverApi.Interfaces;
using MarsRoverApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MarsRoverApi.Extensions;

namespace MarsRoverApi.Services
{
    public class MarsRoverDbRepository: IMarsRoverDbRepository
    {
        private readonly ILogger<MarsRoverDbRepository> _logger;
        private readonly MarsRoverDbContext _marsRoverDbContext;

        public MarsRoverDbRepository(
            ILogger<MarsRoverDbRepository> logger,
            MarsRoverDbContext marsRoverDbContext
            )
        {
            _logger = logger;
            _marsRoverDbContext = marsRoverDbContext;
        }

        public async Task<bool> DoRoversExists()
        {
            var result = await _marsRoverDbContext.Rovers.AsNoTracking().Take(1).FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<IList<Rover>> GetRovers()
        {
            var rovers = await _marsRoverDbContext.Rovers
                .AsNoTracking()
                .Include(r => r.Cameras)
                .ToListAsync();

            return rovers;
        }

        public async Task<Rover> GetRover(int roverId)
        {
            var rover = await _marsRoverDbContext.Rovers
                .AsNoTracking()
                .Where(r => r.Id == roverId)
                .OrderBy(r => r.Id)
                .Include(r => r.Cameras)
                .FirstOrDefaultAsync();

            return rover;
        }

        public async Task<IList<Rover>> CreateRovers(IList<Rover> rovers)
        {
            try
            {
                _marsRoverDbContext.Rovers.AddRange(rovers);
                await _marsRoverDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Failed to save rovers {0}", ex.Message);
                throw; // todo:
            }

            return rovers;
        }

        public async Task<MarsPhoto> CreateMarsPhoto(MarsPhoto marsPhoto)
        {
            try
            {
                _marsRoverDbContext.Add(marsPhoto);
                await _marsRoverDbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to save mars photo to database. Id= {marsPhoto.Id} Error: {ex.Message}");
            }
            return marsPhoto;
        }

        public async Task<int> GetMarsPhotosByRoverDateCount(int roverId, string earthDate)
        {
            var count = await _marsRoverDbContext.MarsPhotos
                .AsNoTracking()
                .Where(p => p.RoverId == roverId && p.EarthDate == earthDate)
                .CountAsync();

            return count;
        }

        public async Task<IList<MarsPhoto>> GetMarsPhotos(int roverId, string earthDate)
        {
            var marsPhotos = await _marsRoverDbContext.MarsPhotos
                .AsNoTracking()
                .Where(p => p.RoverId == roverId && p.EarthDate == earthDate)
                .OrderBy(p => p.Id)
                .ThenBy(p => p.CameraId)
                .Include(p => p.Camera)
                .Include(p => p.Rover)
                .ToListAsync();

            return marsPhotos;
        }


        public async Task<PagedResult<MarsPhoto>> GetMarsPhotosPagedAsync(int roverId, string earthDate, int page, int pageSize)
        {
            var marsPhotosQuery = _marsRoverDbContext.MarsPhotos
                .AsNoTracking()
                .Where(p => p.RoverId == roverId && p.EarthDate == earthDate)
                .OrderBy(p => p.Id)
                .ThenBy(p => p.CameraId)
                .Include(p => p.Camera)
                .Include(p => p.Rover);

            var marsPhotosPaged = await marsPhotosQuery.GetPagedAsync(page, pageSize);

            return marsPhotosPaged;
        }

        public async Task<bool> DoMarsPhotoExists(int nasaPhotoId)
        {
            var count = await _marsRoverDbContext.MarsPhotos
                .AsNoTracking()
                .Where(p => p.NasaPhotoId == nasaPhotoId)
                .CountAsync();
            return count > 0;
        }

        public async Task<IList<MarsPhoto>> GetMarsPhoto(int photoId)
        {
            var marsPhoto = await _marsRoverDbContext.MarsPhotos
                .AsNoTracking()
                .Where(p => p.Id == photoId)
                .Include(p => p.Camera)
                .Include(p => p.Rover)
                .ToListAsync();

            return marsPhoto;
        }
    }
}
