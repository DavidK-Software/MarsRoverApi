using MarsRoverApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(
            IImageService imageService)
        {
            _imageService = imageService;
        }

        // GET: api/media/images/{imagePath}/{imageFileName}
        [HttpGet("{imagePath}/{imageFileName}")]
        public async Task<IActionResult> GetImage(string imagePath, string imageFileName)
        {
            var image = await _imageService.GetImageAsync(imagePath, imageFileName);
            if (image == null)
            {
                return NotFound();
            }

            return File(image, "image/jpeg");
        }

    }
}
