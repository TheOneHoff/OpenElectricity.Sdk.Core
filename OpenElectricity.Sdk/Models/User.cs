using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// User of the OpenElectricity API
    /// </summary>
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public required string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string? OwnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserPlan? Plan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("rate_limit")]
        public UserRateLimit? RateLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserMeta? Meta { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Roles { get; set; } = [];
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserRateLimit
    {
        /// <summary>
        /// Total requests allowed
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Remaining requests allowed
        /// </summary>
        public int Remaining { get; set; }
        /// <summary>
        /// DateTime that the Remaining requests are reset to the Limit
        /// </summary>
        public DateTime Reset { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserMeta
    {
        /// <summary>
        /// Remaining requests allowed
        /// </summary>
        public int Remaining { get; set; }
        /// <summary>
        /// DateTime that the Remaining requests are reset to the Limit
        /// </summary>
        public DateTime Reset { get; set; }
    }
}
