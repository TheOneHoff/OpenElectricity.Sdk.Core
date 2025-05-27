namespace OpenElectricity.Sdk.Models
{
    public class Facility
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Network_Id { get; set; }
        public required string Network_Region { get; set; }
        public string? Description { get; set; }
        public List<Unit> Units { get; set; } = [];
    }
}
