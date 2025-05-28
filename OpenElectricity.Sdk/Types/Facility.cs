using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Types
{
    /// <summary>
    /// Facility response from GetFacilities
    /// </summary>
    public class Facility
    {
        /// <summary>
        /// Facility Code
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Facility Name
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Network code that the facility belongs to
        /// </summary>
        [JsonPropertyName("network_id")]
        public required string Network_Id { get; set; }
        /// <summary>
        /// Network region that the facility belongs to
        /// </summary>
        [JsonPropertyName("network_region")]
        public required string Network_Region { get; set; }
        /// <summary>
        /// Optional Description
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// List of <see cref="Unit"/> within this facility
        /// </summary>
        public List<Unit> Units { get; set; } = [];
    }
}
