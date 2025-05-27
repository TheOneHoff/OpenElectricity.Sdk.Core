using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class TimeSeriesResult
    {
        public required string Name { get; set; }
        [JsonPropertyName("date_start")]
        public DateTimeOffset DateStart { get; set; }
        [JsonPropertyName("date_end")]
        public DateTimeOffset DateEnd { get; set; }
        public Dictionary<string, string> Columns { get; set; } = [];
        public List<List<JsonElement>> Data { get; set; } = [];

        [JsonIgnore]
        public List<TimeValue> TimeSeries
        {
            get
            {
                List<TimeValue> timeSeries = [];
                foreach(var entry in Data)
                {
                    DateTimeOffset timestamp = entry[0].GetDateTimeOffset();
                    decimal value = entry[1].GetDecimal();
                    timeSeries.Add(new(timestamp, value));
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
