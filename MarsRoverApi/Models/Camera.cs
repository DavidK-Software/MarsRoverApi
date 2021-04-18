using System.ComponentModel.DataAnnotations;

namespace MarsRoverApi.Models
{
    public class Camera
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoverId { get; set; }
        public string FullName { get; set; }
    }
}
