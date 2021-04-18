using System.Collections.Generic;

namespace MarsRoverApi.Interfaces
{
    public interface IDateService
    {
        IEnumerable<string> ReadDates();
    }
}
