using MarsRoverApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarsRoverApi.Services
{
    public class InitializationService
    {
        private readonly IMarsPhotoRetrievalService _marsPhotoRetrievalService;
        private readonly ILogger<InitializationService> _logger;
        private readonly IDateService _dateService;

        public InitializationService(
                ILogger<InitializationService> logger,
                IMarsPhotoRetrievalService marsPhotoService,
                IDateService dateService
            )
        {
            _logger = logger;
            _marsPhotoRetrievalService = marsPhotoService;
            _dateService = dateService;
        }

        public void Initialize()
        {
            _logger.LogInformation("Initialializing data");

            try
            {
                _marsPhotoRetrievalService.EnsureImagePathCreated();

                var rovers = _marsPhotoRetrievalService.InitializeRovers().Result;
                _logger.LogInformation($"Loaded {rovers} mars rovers from Nasa API");

                foreach (var earthDate in _dateService.ReadDates())
                {
                    var photosCount = _marsPhotoRetrievalService.SaveAllRoverMarsPhotosAsync(earthDate).Result;
                    _logger.LogInformation($" Loaded {photosCount} Mars photos from Nasa API");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed to initialize Mars photos from Nasa API", ex.Message);
                throw;
            }
        }

    }
}
