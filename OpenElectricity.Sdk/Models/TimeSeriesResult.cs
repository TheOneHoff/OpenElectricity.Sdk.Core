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
        /// Time series data
        /// </summary>
        public List<TimeValue> Data { get; set; } = [];
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
