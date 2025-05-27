using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk
{
    public class StaticJsonSerializerOptions
    {
        private JsonSerializerOptions? _default;
        public JsonSerializerOptions Default
        {
            get
            {
                if (_default is null)
                {
                    _default = new(JsonSerializerDefaults.Web)
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    };
                    _default.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, false));
                }
                return _default;
            }
        }
    }
}
