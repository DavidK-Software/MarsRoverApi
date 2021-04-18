using MarsRoverApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoversController : ControllerBase
    {
        private readonly IMarsRoverDbRepository _marsRoverDbRepository;

        public RoversController(
            IMarsRoverDbRepository marsRoverDbRepository)
        {
            _marsRoverDbRepository = marsRoverDbRepository;
        }

        // GET: api/rovers
        [HttpGet]
        public async Task<IActionResult> GetRovers()
        {
            var rovers = await _marsRoverDbRepository.GetRovers();
            return Ok(rovers);
        }

        // GET: api/rovers/{roverid}
        [HttpGet("{roverid}")]
        public async Task<IActionResult> GetRover(int roverid)
        {
            var rover = await _marsRoverDbRepository.GetRover(roverid);
            return Ok(rover);
        }

        // GET: api/rovers/{roverid}/photos/earthdate/?page=2&pagesize=10
        [HttpGet("{roverid}/photos/{earthdate}")]
        public async Task<IActionResult> GetMarsPhotos(int roverid, string earthdate, int page = 1, int pagesize = 10)
        {
            var marsPhotos = await _marsRoverDbRepository.GetMarsPhotosPagedAsync(roverid, earthdate, page, pagesize);
            return Ok(marsPhotos);
        }

    }
}
