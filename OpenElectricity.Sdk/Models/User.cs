using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public class User
    {
        public required string Id { get; set; }
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
        public string? Email { get; set; }
        [JsonPropertyName("owner_id")]
        public string? OwnerId { get; set; }
        public UserPlan? Plan { get; set; }
        [JsonPropertyName("rate_limit")]
        public UserRateLimit? RateLimit { get; set; }
        public object? Meta { get; set; }
        public List<string> Roles { get; set; } = [];
    }

    public class UserRateLimit
    {
        public int Limit { get; set; }
        public int Remaining { get; set; }
        public DateTime Reset { get; set; }
    }
}
