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
            DateTimeOffset? dateTimeOffset = null;
            decimal? value = null;

            while(reader.Read())
            {
                JsonElement element;
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        continue;
                    case JsonTokenType.String:
                        element = JsonElement.ParseValue(ref reader);
                        dateTimeOffset = element.GetDateTimeOffset();
                        break;
                    case JsonTokenType.Number:
                        element = JsonElement.ParseValue(ref reader);
                        value = element.GetDecimal();
                        break;
                    case JsonTokenType.EndArray:
                        if (dateTimeOffset is null || value is null)
                        {
                            throw new JsonException();
                        }
                        return new(dateTimeOffset.Value, value.Value);
                    default:
                        throw new JsonException();
                }
            }
            throw new JsonException();
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
