using System.Text.Json;
using System.Text.Json.Serialization;
using OpenElectricity.Sdk.Models;

namespace OpenElectricity.Sdk.Helpers
{
    internal class TimeValueJsonConverter : JsonConverter<TimeValue>
    {
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
            DateTimeOffset dateTimeOffset = reader.GetDateTimeOffset();
            reader.Read();
            decimal value = reader.GetDecimal();
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return new(dateTimeOffset, value);
        }

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
