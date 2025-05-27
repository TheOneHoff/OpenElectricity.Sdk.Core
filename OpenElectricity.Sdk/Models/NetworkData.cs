using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class NetworkData
    {
        [JsonPropertyName("network_code")]
        public required string NetworkCode { get; set; }
        public required Metric Metric { get; set; }
        public required string Unit { get; set; }
        public required DataInterval Interval { get; set; }
        [JsonPropertyName("date_start")]
        public required DateTimeOffset DateStart { get; set; }
        [JsonPropertyName("date_end")]
        public required DateTimeOffset DateEnd { get; set; }
        public List<string> Groupings { get; set; } = [];
        public List<TimeSeriesResult> Results { get; set; } = [];
        [JsonPropertyName("network_timezone_offset")]
        public string? NetworkTimezoneOffset { get; set; }
    }
}
