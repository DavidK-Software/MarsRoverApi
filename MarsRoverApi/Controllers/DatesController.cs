using MarsRoverApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatesController : ControllerBase
    {
        private readonly IDateService _dateService;

        public DatesController(
            IDateService dateService
            )
        {
            _dateService = dateService;
        }

        // GET: api/<DatesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var dates = _dateService.ReadDates();

            return dates;
        }

    }
}
