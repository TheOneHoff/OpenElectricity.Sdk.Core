using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class Unit
    {
        public required string Code { get; set; }
        [JsonPropertyName("fueltech_id")]
        public UnitFueltechType? FueltechId { get; set; }
        [JsonPropertyName("status_id")]
        public UnitStatusType? StatusId { get; set; }
        [JsonPropertyName("capacity_registered")]
        public decimal? CapacityRegistered { get; set; }
        [JsonPropertyName("emissions_factor_co2")]
        public decimal? EmissionsFactorCo2 { get; set; }
        [JsonPropertyName("data_first_seen")]
        public DateTimeOffset? DataFirstSeen { get; set; }
        [JsonPropertyName("data_last_seen")]
        public DateTimeOffset? DataLastSeen { get; set; }
        [JsonPropertyName("dispatch_type")]
        public UnitDispatchType? DispatchType { get; set; }
    }
}
