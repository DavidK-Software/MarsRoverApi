using AutoMapper;
using MarsRoverApi.Infrastructure;
using MarsRoverApi.Models;
using NasaApiLib.Models;
using Xunit;

namespace MarsRoverApi.Test
{
    public class UnitTestMappings
    {
        [Fact]
        public void Test_Configuration()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Test_MarsPhotoMapping()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            config.AssertConfigurationIsValid();

            NasaMarsPhoto nasaMarsPhoto = new NasaMarsPhoto()
            {
                Id = 617458,
                Sol = 1622,
                Camera = new NasaCamera()
                {
                    Id = 20,
                    Name = "FHAZ",
                    RoverId = 5,
                    FullName = "Front Hazard Avoidance Camera"
                },
                ImgSrc = "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01622/opgs/edr/fcam/FLB_541484941EDR_F0611140FHAZ00341M_.JPG",
                EarthDate = "2017-02-27",
                Rover = new NasaRover()
                {
                    Id = 5,
                    Name = "Curiosity",
                    LandingDate = "2012-08-06",
                    LaunchDate = "2011-11-26",
                    Status = "active"
                }
            };

            var marsPhoto = mapper.Map<MarsPhoto>(nasaMarsPhoto);

            Assert.Equal(nasaMarsPhoto.Id, marsPhoto.Id);
            Assert.Equal(nasaMarsPhoto.ImgSrc, marsPhoto.ImgSrc);
            Assert.Equal(nasaMarsPhoto.EarthDate, marsPhoto.EarthDate);

        }
    }
}
