using System.Text.Json;

namespace OpenElectricity.Sdk
{
    public class OpenElectricityOptions
    {
        public required string ApiKey { get; set; }
        public Uri BaseUrl { get; set; } = new("https://api.openelectricity.org.au/v4/");
    }
}
