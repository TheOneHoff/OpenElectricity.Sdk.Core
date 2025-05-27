using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// Response from any time-series routes
    /// </summary>
    public class NetworkData
    {
        /// <summary>
        /// Network code 
        /// </summary>
        [JsonPropertyName("network_code")]
        public required string NetworkCode { get; set; }
        /// <summary>
        /// Requested metric
        /// </summary>
        public required Metric Metric { get; set; }
        /// <summary>
        /// Units of measurement for this data
        /// </summary>
        public required string Unit { get; set; }
        /// <summary>
        /// The time interval the data is aggregated by
        /// </summary>
        public required DataInterval Interval { get; set; }
        /// <summary>
        /// The start date of the data
        /// </summary>
        [JsonPropertyName("date_start")]
        public required DateTimeOffset DateStart { get; set; }
        /// <summary>
        /// The end date of the data
        /// </summary>
        [JsonPropertyName("date_end")]
        public required DateTimeOffset DateEnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Groupings { get; set; } = [];
        /// <summary>
        /// List of results
        /// </summary>
        public List<TimeSeriesResult> Results { get; set; } = [];
        /// <summary>
        /// The UTC offset of the timezone of this network data
        /// </summary>
        [JsonPropertyName("network_timezone_offset")]
        public string? NetworkTimezoneOffset { get; set; }
    }
}
