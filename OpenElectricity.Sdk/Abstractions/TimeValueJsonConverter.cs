using System.Text.Json;
using System.Text.Json.Serialization;
using OpenElectricity.Sdk.Types;

namespace OpenElectricity.Sdk.Abstractions
{
    /// <summary>
    /// Custom JSON converter for <see cref="TimeValue"/>
    /// </summary>
    public class TimeValueJsonConverter : JsonConverter<TimeValue>
    {
        /// <summary>
        /// Reads and converts the JSON to type <see cref="TimeValue"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns>The converted value</returns>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public override TimeValue Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            var dateTimeOffset = reader.GetDateTimeOffset();
            reader.Read();
            decimal value = reader.GetDecimal();
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return new(dateTimeOffset, value);
        }

        /// <summary>
        /// Writes a specified <see cref="TimeValue"/> as JSON
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public override void Write(
            Utf8JsonWriter writer,
            TimeValue value, 
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(value.Timestamp.ToString("s"));
            writer.WriteNumberValue(value.Value);
            writer.WriteEndArray();
        }
    }
}
