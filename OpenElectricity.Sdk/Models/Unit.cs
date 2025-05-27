using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// A Generation Unit of a <see cref="Facility"/>
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Unit Code
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("fueltech_id")]
        public UnitFueltechType? FueltechId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("status_id")]
        public UnitStatusType? StatusId { get; set; }
        /// <summary>
        /// The amount in MW that this unit can generate at max
        /// </summary>
        [JsonPropertyName("capacity_registered")]
        public decimal? CapacityRegistered { get; set; }
        /// <summary>
        /// The CO2 emissions factor for this unit
        /// </summary>
        [JsonPropertyName("emissions_factor_co2")]
        public decimal? EmissionsFactorCo2 { get; set; }
        /// <summary>
        /// All time start of data for this unit
        /// </summary>
        [JsonPropertyName("data_first_seen")]
        public DateTimeOffset? DataFirstSeen { get; set; }
        /// <summary>
        /// All time end of data for this unit
        /// </summary>
        [JsonPropertyName("data_last_seen")]
        public DateTimeOffset? DataLastSeen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("dispatch_type")]
        public UnitDispatchType? DispatchType { get; set; }
    }
}
