namespace MarsRoverApi.Models
{
    public class MarsPhoto
    {
        public int Id { get; set; }
        public int NasaPhotoId { get; set; }
        public int Sol { get; set; }
        public string ImgSrc { get; set; }
        public string EarthDate { get; set; }

        public int CameraId { get; set; }
        public Camera Camera { get; set; }

        public int RoverId { get; set; }
        public Rover Rover { get; set; }
    }
}
