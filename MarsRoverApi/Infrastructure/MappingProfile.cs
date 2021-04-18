using AutoMapper;
using MarsRoverApi.Models;
using NasaApiLib.Models;

namespace MarsRoverApi.Infrastructure
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<NasaRover, Rover>().ReverseMap();
            CreateMap<NasaCamera, Camera>().ReverseMap();
            CreateMap<NasaMarsPhoto, MarsPhoto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NasaPhotoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CameraId, opt => opt.MapFrom(src => src.Camera.Id))
                .ForMember(dest => dest.Camera, opt => opt.Ignore())
                .ForMember(dest => dest.RoverId, opt => opt.MapFrom(src => src.Rover.Id))
                .ForMember(dest => dest.Rover, opt => opt.Ignore());
        }
    }
}
