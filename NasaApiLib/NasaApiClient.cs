using Microsoft.Extensions.Logging;
using NasaApiLib.Interfaces;
using NasaApiLib.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NasaApiLib
{
    public class NasaApiClient : INasaApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NasaApiClient> _logger;
        private readonly INasaApiSettings _settings;

        public NasaApiClient(
            HttpClient httpClient,
            ILogger<NasaApiClient> logger,
            INasaApiSettings settings
            )
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<NasaMarsRovers> GetRoversAsync()
        {
            NasaMarsRovers marsRovers;

            var uri = $"{_settings.NasaApiUrl}/{_settings.MarsRoverApi}/?&api_key={_settings.NasaApiKey}";

            _logger.LogInformation($"GetRoversAsync");

            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync();
            marsRovers = JsonSerializer.Deserialize<NasaMarsRovers>(content);

            return marsRovers;
        }

        public async Task<IList<NasaMarsPhoto>> GetRoverPhotosAsync(string roverName, string earthDate, int page)
        {
            var uri = $"{_settings.NasaApiUrl}/{_settings.MarsRoverApi}/{roverName}/photos?earth_date={earthDate}&page={page}&api_key={_settings.NasaApiKey}";

            _logger.LogInformation($"GetRoverPhotosAsync roverName: {roverName} earthDate: {earthDate} page:{page}");

            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync();
            var nasaMarsPhotos = JsonSerializer.Deserialize<NasaMarsPhotos>(content);

            var marsPhotos = nasaMarsPhotos.Photos;

            return marsPhotos;
        }

        public async Task<byte[]> GetRoverPhotoAsync(string imgSrc)
        {
            byte[] image;

            var response = await _httpClient.GetAsync(imgSrc);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
            }

            image = await response.Content.ReadAsByteArrayAsync();
            _logger.LogInformation($"GetRoverPhotoAsync ImgSrc: {imgSrc}");

            return image;
        }
    }
}
