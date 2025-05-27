using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class TimeSeriesResult
    {
        public required string Name { get; set; }
        public DateTimeOffset Date_Start { get; set; }
        public DateTimeOffset Date_End { get; set; }
        public Dictionary<string, string> Columns { get; set; } = [];
        public List<List<object>> Data { get; set; } = [];

        [JsonIgnore]
        public List<TimeValue> TimeSeries
        {
            get
            {
                List<TimeValue> timeSeries = [];
                foreach(var entry in Data)
                {
                    timeSeries.Add(new((DateTimeOffset)entry[0], (decimal)entry[1]));
                }
                return timeSeries;
            }
        }
    }

    public struct TimeValue(DateTimeOffset timestamp, decimal value)
    {
        public DateTimeOffset Timestamp { get; set; } = timestamp;
        public decimal Value { get; set; } = value;
    }
}
