namespace OpenElectricity.Sdk.Models
{
    public class Unit
    {
        public required string Code { get; set; }
        public UnitFueltechType? Fueltech_Id { get; set; }
        public UnitStatusType? Status_Id { get; set; }
        public decimal Capacity_Registered { get; set; }
        public decimal Emissions_Factor_Co2 { get; set; }
        public required DateTime Data_First_Seen { get; set; }
        public required DateTime Data_Last_Seen { get; set; }
        public UnitDispatchType? Dispatch_Type { get; set; }
    }
}
