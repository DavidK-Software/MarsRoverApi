using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NasaApiLib.Models
{
    public class NasaRover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("landing_date")]
        public string LandingDate { get; set; }
        [JsonPropertyName("launch_date")]
        public string LaunchDate { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("cameras")]
        public List<NasaCamera> Cameras { get; set; }
    }
}
