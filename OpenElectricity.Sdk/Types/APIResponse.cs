using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Types
{
    internal class APIResponse<T>
    {
        public string? Version { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Success { get; set; } = true;
        public string? Error { get; set; }
        public T? Data { get; set; }
        [JsonPropertyName("total_records")]
        public int? TotalRecords { get; set; }
    }
}
