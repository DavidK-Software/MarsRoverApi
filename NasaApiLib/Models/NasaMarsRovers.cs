using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NasaApiLib.Models
{
    public class NasaMarsRovers
    {
        [JsonPropertyName("rovers")]
        public List<NasaRover> Rovers { get; set; }
    }
}