using AutoMapper;
using MarsRoverApi.Interfaces;
using MarsRoverApi.Models;
using Microsoft.Extensions.Logging;
using NasaApiLib.Interfaces;
using NasaApiLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public class MarsPhotoRetrievalService : IMarsPhotoRetrievalService
    {
        private readonly ILogger<MarsPhotoRetrievalService> _logger;
        private readonly INasaApiClient _nasaApiClient;
        private readonly IMarsRoverDbRepository _marsRoverDbRepository;
        private readonly IImageService _imageService;
        private readonly IImagePaths _imagePaths;
        private readonly IMapper _mapper;

        public MarsPhotoRetrievalService (
            ILogger<MarsPhotoRetrievalService> logger,
            INasaApiClient nasaApiClient,
            IMarsRoverDbRepository marsRoverDbRepository,
            IImageService imageService,
            IImagePaths imagePaths,
            IMapper mapper
            )
        {
            _logger = logger;
            _nasaApiClient = nasaApiClient;
            _marsRoverDbRepository = marsRoverDbRepository;
            _imageService = imageService;
            _imagePaths = imagePaths;
            _mapper = mapper;
        }

        public bool EnsureImagePathCreated()
        {
            return _imageService.EnsureImagePathCreated(_imagePaths.MarsRoverImagePath);
        }

        public async Task<IList<Rover>> InitializeRovers()
        {
            IList<Rover> dbRovers = null;
            if (!await _marsRoverDbRepository.DoRoversExists())
            {
                NasaMarsRovers nasaMarsRovers = await _nasaApiClient.GetRoversAsync();
                List<Rover> rovers = _mapper.Map<List<Rover>>(nasaMarsRovers.Rovers);
                dbRovers = await _marsRoverDbRepository.CreateRovers(rovers);
            }

            return dbRovers;
        }


        public async Task<int> SaveRoverMarsPhotosAsync(Rover rover, string earthDate)
        {
            var photosCount = 0;

            try
            {
                // If data already exists, do not pull for that rover and earthdate again
                var roverspictures = await _marsRoverDbRepository.GetMarsPhotos(rover.Id, earthDate);
                var roverPhotosCount = await  _marsRoverDbRepository.GetMarsPhotosByRoverDateCount(rover.Id, earthDate);
                if (roverPhotosCount == 0)
                {
                    var page = 1;
                    IList<NasaMarsPhoto> nasaMarsPhotos = null;
                    do
                    {
                        nasaMarsPhotos = await _nasaApiClient.GetRoverPhotosAsync(rover.Name, earthDate, page++);
                        if (nasaMarsPhotos != null && nasaMarsPhotos.Count != 0)
                        {
                            _logger.LogInformation($"Initialializing photos for Rover: {rover.Name} Date: {earthDate} Page: {page}  Count: {nasaMarsPhotos.Count}");

                            photosCount += nasaMarsPhotos.Count;
                            foreach (var nasaMarsPhoto in nasaMarsPhotos)
                            {
                                // If the image was previously saved, do not save again
                                var photoExists = await _marsRoverDbRepository.DoMarsPhotoExists(nasaMarsPhoto.Id);
                                if (!photoExists)
                                {
                                    var imagePath = await SaveMarsPhotoImageAsync(nasaMarsPhoto);
                                    if (imagePath != null)
                                    {
                                        var marsPhoto = _mapper.Map<MarsPhoto>(nasaMarsPhoto);
                                        marsPhoto.ImgSrc = imagePath;
                                        var createdMarsPhoto = await _marsRoverDbRepository.CreateMarsPhoto(marsPhoto);
                                    }
                                } 
                                else
                                {
                                    _logger.LogInformation($"Photo already saved...skippping Rover: {rover.Name} Date: {earthDate} Page: {page}  Id: {nasaMarsPhoto.Id}");
                                }
                            };
                        }

                    } while (nasaMarsPhotos != null && nasaMarsPhotos.Count != 0);
                }
                else
                {
                    _logger.LogInformation($"Photos already saved for Rover: {rover.Name} Date: {earthDate}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save images for Rover. {rover} Earthdate: {earthDate} Error: {ex.Message}");
                throw;
            }

            return photosCount;
        }


        public async Task<int> SaveAllRoverMarsPhotosAsync(string earthDate)
        {
            var photosCount = 0;
            IList<Rover> rovers;
            try
            {
                rovers = _marsRoverDbRepository.GetRovers().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve rovers. Error: {ex.Message}");
                throw;
            }

            foreach (var rover in rovers)
            {
                try
                {
                    photosCount += await SaveRoverMarsPhotosAsync(rover, earthDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to save mars photos for rovers {rover.Name}. Error: {ex.Message}");
                    throw;
                }
            }

            return photosCount;
        }

        protected async Task<string> SaveMarsPhotoImageAsync(NasaMarsPhoto nasaMarsPhoto)
        {
            string imagePath = null;
            _logger.LogInformation($"SaveMarsPhotoImageAsync Id: {nasaMarsPhoto.Id} ImgSrc: {nasaMarsPhoto.ImgSrc}");

            try
            {
                byte[] image = await _nasaApiClient.GetRoverPhotoAsync(nasaMarsPhoto.ImgSrc);

                var uri = new Uri(nasaMarsPhoto.ImgSrc);
                var imageFileName = uri.Segments.LastOrDefault();

                if (imageFileName != null)
                {
                    imagePath = await _imageService.SaveImageAsync(_imagePaths.MarsRoverImagePath, imageFileName, image);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save image Id: {nasaMarsPhoto.Id} ImgSrc: {nasaMarsPhoto.ImgSrc} Error: {ex.Message}");
            }

            return imagePath;
        }
    }
}
