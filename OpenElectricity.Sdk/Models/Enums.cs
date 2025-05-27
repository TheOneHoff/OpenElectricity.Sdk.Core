using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    public enum NetworkCode
    {
        NEM,
        WEM,
        AU
    }

    public enum DataInterval
    {
        [JsonStringEnumMemberName("5m")]
        FiveMinute,
        [JsonStringEnumMemberName("1h")]
        OneHour,
        [JsonStringEnumMemberName("7d")]
        SevenDay,
        [JsonStringEnumMemberName("1M")]
        OneMonth,
        [JsonStringEnumMemberName("3M")]
        ThreeMonth,
        [JsonStringEnumMemberName("season")]
        Season,
        [JsonStringEnumMemberName("1y")]
        OneYear,
        [JsonStringEnumMemberName("fy")]
        FinancialYear
    }

    public static class DataIntervalExtensions
    {
        public static string ToJsonString(this DataInterval dataInterval)
        {
            Type type = typeof(DataInterval);
            string? name = type.GetEnumName(dataInterval);
            if (name is null) return "";
            return type.GetField(name)?
                .GetCustomAttributes(false)
                .OfType<JsonStringEnumMemberNameAttribute>()
                .SingleOrDefault()?
                .ToString() ?? "";
        }
    }

    public enum DataPrimaryGrouping
    {
        network,
        network_region
    }

    public enum DataSecondaryGrouping
    {
        fueltech,
        fueltech_group,
        renewable
    }
    public enum Metric
    {
        power,
        energy,
        emissions,
        market_value,
        price,
        demand,
        demand_energy
    }

    public enum DataMetric
    {
        power,
        energy,
        emissions,
        market_value
    }

    public enum MarketMetric
    {
        price,
        demand,
        demand_energy
    }

    public enum UnitStatusType
    {
        committed,
        operating,
        retired
    }

    public enum UnitFueltechType
    {
        battery,
        battery_charging,
        battery_discharging,
        bioenergy_biogas,
        bioenergy_biomass,
        coal_black,
        coal_brown,
        distillate,
        gas_ccgt,
        gas_ocgt,
        gas_recip,
        gas_steam,
        gas_wcmg,
        hydro,
        pumps,
        solar_rooftop,
        solar_thermal,
        solar_utility,
        nuclear,
        other,
        solar,
        wind,
        wind_offshore,
        imports,
        exports,
        interconnector,
        aggregator_vpp,
        aggregator_dr
    }

    public enum UnitFueltechGroupType
    {
        coal,
        gas,
        wind,
        solar,
        battery_charging,
        battery_discharging,
        hydro,
        distillate,
        bioenergy,
        pumps
    }

    public enum UnitDispatchType
    {
        GENERATOR,
        LOAD,
        NETWORK,
        INTERCONNECTOR
    }

    public enum UserPlan
    {
        BASIC,
        PRO,
        ENTERPRISE
    }
}
