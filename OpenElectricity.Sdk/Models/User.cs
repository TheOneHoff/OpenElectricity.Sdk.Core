namespace OpenElectricity.Sdk.Models
{
    public class User
    {
        public required string Id { get; set; }
        public string? Full_Name { get; set; }
        public string? Email { get; set; }
        public string? Owner_Id { get; set; }
        public UserPlan? Plan { get; set; }
        public UserRateLimit? Rate_Limit { get; set; }
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
