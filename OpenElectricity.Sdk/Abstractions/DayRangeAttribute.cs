namespace OpenElectricity.Sdk.Abstractions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class DayRangeAttribute(int days) : Attribute
    {
        public int Days { get; set; } = days;
    }
}
