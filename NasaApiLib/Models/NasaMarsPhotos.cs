using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NasaApiLib.Models
{
    public class NasaMarsPhotos
    {
        [JsonPropertyName("photos")]
        public List<NasaMarsPhoto> Photos { get; set; }
    }
}
