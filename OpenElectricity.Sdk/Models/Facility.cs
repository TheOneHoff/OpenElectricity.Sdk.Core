using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class Facility
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        [JsonPropertyName("network_id")]
        public required string Network_Id { get; set; }
        [JsonPropertyName("network_region")]
        public required string Network_Region { get; set; }
        public string? Description { get; set; }
        public List<Unit> Units { get; set; } = [];
    }
}
