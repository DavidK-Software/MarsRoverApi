using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarsRoverApi.Models
{
    public class Rover
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LandingDate { get; set; }
        public string LaunchDate { get; set; }
        public string Status { get; set; }

        public List<Camera> Cameras { get; set; }

    }
}
