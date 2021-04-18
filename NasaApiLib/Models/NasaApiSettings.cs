using NasaApiLib.Interfaces;

namespace NasaApiLib.Models
{
    public class NasaApiSettings : INasaApiSettings
    {
        public string NasaApiUrl { get; set; }
        public string NasaApiKey { get; set; }
        public string MarsRoverApi { get; set; }
    }
}
