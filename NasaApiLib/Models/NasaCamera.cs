using System.Text.Json.Serialization;

namespace NasaApiLib.Models
{
    public class NasaCamera
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("rover_id")]
        public int RoverId { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
    }
}
