using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Helpers
{
    internal static class StaticJsonSerializerOptions
    {
        public static JsonSerializerOptions GetDefaultOptions()
        {
            JsonSerializerOptions _default = new(JsonSerializerDefaults.Web)
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            _default.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, false));
            _default.Converters.Add(new TimeValueJsonConverter());

            return _default;
        }
    }
}
