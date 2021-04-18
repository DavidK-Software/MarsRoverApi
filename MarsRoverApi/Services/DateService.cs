using MarsRoverApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MarsRoverApi.Services
{
    public class DateService: IDateService
    {
        private readonly ILogger<DateService> _logger;
        protected readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public DateService(
            ILogger<DateService> logger,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public IEnumerable<string> ReadDates()
        {
            var dateFileName = _configuration["DateFileName"];

            var path = Path.Combine(_hostEnvironment.ContentRootPath, $"Resources/{dateFileName}");
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var styles = DateTimeStyles.AllowWhiteSpaces;

            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while((line = sr.ReadLine()) != null)
                {
                    _logger.LogInformation(line);

                    DateTime dateTime;
                    if (DateTime.TryParse(line, culture, styles, out dateTime))
                    {
                        yield return dateTime.ToString("yyyy-MM-dd", culture);
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to parse date: {line}");
                    }
                }
            }
        }

    }
}
