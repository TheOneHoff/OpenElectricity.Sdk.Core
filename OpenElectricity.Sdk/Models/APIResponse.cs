namespace OpenElectricity.Sdk.Models
{
    internal class APIResponse<T>
    {
        public string? Version { get; set; }
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public bool Success { get; set; } = true;
        public string? Error { get; set; }
        public T? Data { get; set; }
        public int? Total_Records { get; set; }
    }
}
