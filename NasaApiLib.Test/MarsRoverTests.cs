using Microsoft.Extensions.Logging.Abstractions;
using NasaApiLib.Interfaces;
using NasaApiLib.Models;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace NasaApiLib.Test
{
    public class MarsRoverTests
    {
        [Theory]
        [InlineData("curiosity", "2017-02-27", 1, 25)]
        public void Test_GetPhotos01(string roverName, string date, int page, int expectedCount)
        {
            var logger = new NullLogger<NasaApiClient>();

            INasaApiSettings settings = new NasaApiSettings()
            {
                NasaApiUrl = "https://api.nasa.gov",
                MarsRoverApi = "mars-photos/api/v1/rovers",
                NasaApiKey = "b4rSDXQkrk4eKsUcU8zBfXykwe9cktbqg78Nmg3x"
            };

            using (HttpClient _httpClient = new HttpClient())
            {
                INasaApiClient client = new NasaApiClient(_httpClient, logger, settings);
                IList<NasaMarsPhoto> nasaMarsPhotos = client.GetRoverPhotosAsync(roverName, date, page).Result;

                Assert.NotNull(nasaMarsPhotos);
                Assert.Equal(expectedCount, nasaMarsPhotos.Count);
                Assert.Equal(date, nasaMarsPhotos[0].EarthDate);
            }

        }

        [Theory]
        [InlineData("curiosity", "2017-02-27", 2, 25)]
        [InlineData("curiosity", "2017-02-27", 3, 1)]
        public void Test_GetPhotos02(string roverName, string date, int page, int expectedCount)
        {
            var logger = new NullLogger<NasaApiClient>();

            INasaApiSettings settings = new NasaApiSettings()
            {
                NasaApiUrl = "https://api.nasa.gov",
                MarsRoverApi = "mars-photos/api/v1/rovers",
                NasaApiKey = "b4rSDXQkrk4eKsUcU8zBfXykwe9cktbqg78Nmg3x"
            };

            using (HttpClient _httpClient = new HttpClient())
            {
                INasaApiClient client = new NasaApiClient(_httpClient, logger, settings);

                IList<NasaMarsPhoto> nasaMarsPhotos = client.GetRoverPhotosAsync(roverName, date, page).Result;

                Assert.NotNull(nasaMarsPhotos);
                Assert.True(nasaMarsPhotos.Count < expectedCount);
                if (nasaMarsPhotos.Count > 0)
                {
                    Assert.Equal(date, nasaMarsPhotos[0].EarthDate);
                }
            }
        }

        [Theory]
        [InlineData("http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01622/opgs/edr/fcam/FLB_541484941EDR_F0611140FHAZ00341M_.JPG")]
        public void Test_GetImage01(string imgSrc)
        {
            var logger = new NullLogger<NasaApiClient>();

            INasaApiSettings settings = new NasaApiSettings()
            {
                NasaApiUrl = "https://api.nasa.gov",
                MarsRoverApi = "mars-photos/api/v1/rovers",
                NasaApiKey = "b4rSDXQkrk4eKsUcU8zBfXykwe9cktbqg78Nmg3x"
            };

            using (HttpClient _httpClient = new HttpClient())
            {
                INasaApiClient client = new NasaApiClient(_httpClient, logger, settings);

                byte[] images = client.GetRoverPhotoAsync(imgSrc).Result;

                Assert.NotNull(imgSrc);
            }

        }

        [Theory]
        [InlineData(4, 7)]
        public void Test_GetMarsRovers(int expectedRoverCount, int expectedCuriosityCameras)
        {
            var logger = new NullLogger<NasaApiClient>();

            INasaApiSettings settings = new NasaApiSettings()
            {
                NasaApiUrl = "https://api.nasa.gov",
                MarsRoverApi = "mars-photos/api/v1/rovers",
                NasaApiKey = "b4rSDXQkrk4eKsUcU8zBfXykwe9cktbqg78Nmg3x"
            };

            using (HttpClient _httpClient = new HttpClient())
            {
                INasaApiClient client = new NasaApiClient(_httpClient, logger, settings);

                NasaMarsRovers nasaMarsRovers = client.GetRoversAsync().Result;

                Assert.NotNull(nasaMarsRovers);
                Assert.NotNull(nasaMarsRovers.Rovers);
                Assert.Equal(expectedRoverCount, nasaMarsRovers.Rovers.Count);
                if (nasaMarsRovers.Rovers.Count > 0)
                {
                    Assert.Equal("Curiosity", nasaMarsRovers.Rovers[0].Name);
                    Assert.Equal(expectedCuriosityCameras, nasaMarsRovers.Rovers[0].Cameras.Count);
                }
            }

        }
    }
}
