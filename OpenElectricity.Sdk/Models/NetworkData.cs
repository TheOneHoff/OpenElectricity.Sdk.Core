namespace OpenElectricity.Sdk.Models
{
    public class NetworkData
    {
        public required string Network_Code { get; set; }
        public required Metric Metric { get; set; }
        public required string Unit { get; set; }
        public required DataInterval Interval { get; set; }
        public required DateTimeOffset Date_Start { get; set; }
        public required DateTimeOffset Date_End { get; set; }
        public List<string> Groupings { get; set; } = [];
        public List<TimeSeriesResult> Results { get; set; } = [];
        public string? Network_Timezone_Offset { get; set; }
    }
}
