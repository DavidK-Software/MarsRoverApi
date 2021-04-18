namespace NasaApiLib.Interfaces
{
    public interface INasaApiSettings
    {
        string NasaApiUrl { get; set; }
        string NasaApiKey { get; set; }
        string MarsRoverApi { get; set; }
    }
}
