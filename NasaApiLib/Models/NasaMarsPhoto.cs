using System.Text.Json.Serialization;

namespace NasaApiLib.Models
{
    public class NasaMarsPhoto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("sol")]
        public int Sol { get; set; }
        [JsonPropertyName("camera")]
        public NasaCamera Camera { get; set; }
        [JsonPropertyName("img_src")]
        public string ImgSrc { get; set; }
        [JsonPropertyName("earth_date")]
        public string EarthDate { get; set; }
        [JsonPropertyName("rover")]
        public NasaRover Rover { get; set; }
    }
}
