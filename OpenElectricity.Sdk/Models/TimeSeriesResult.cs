using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeSeriesResult
    {
        /// <summary>
        /// Name of this group of data. Composed of the <see cref="Metric"/>, <see cref="DataPrimaryGrouping"/> 
        /// and <see cref="DataSecondaryGrouping"/> used when querying the data
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("date_start")]
        public DateTimeOffset DateStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("date_end")]
        public DateTimeOffset DateEnd { get; set; }
        /// <summary>
        /// Describes the values for this group of data. Length is determined by the number of groupings in the query
        /// </summary>
        public Dictionary<string, string> Columns { get; set; } = [];
        /// <summary>
        /// Structure used to deserialize the time series data using the built in deserializer.
        /// Use the <see cref="TimeSeries"/> property if you want to work with the timeseries data.
        /// This property will be deprecated in the next release.
        /// </summary>
        public List<List<JsonElement>> Data { get; set; } = [];

        /// <summary>
        /// Converts the <see cref="Data"/> property to a time series list.
        /// </summary>
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

    /// <summary>
    /// Struct for storing a related <see cref="DateTimeOffset"/> and a <see cref="decimal"/>
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="value"></param>
    public struct TimeValue(DateTimeOffset timestamp, decimal value)
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset Timestamp { get; set; } = timestamp;
        /// <summary>
        /// 
        /// </summary>
        public decimal Value { get; set; } = value;
    }
}
