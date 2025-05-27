using System.Text.Json.Serialization;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// The id of the electrical network.
    /// <see href="https://docs.openelectricity.org.au/guides/networks"/>
    /// </summary>
    public enum NetworkCode
    {
        /// <summary>
        /// National Electricity Market
        /// </summary>
        NEM,

        /// <summary>
        /// Wholesale Electricity Market
        /// </summary>
        WEM,

        /// <summary>
        /// Australia
        /// </summary>
        AU
    }

    /// <summary>
    /// The time interval to aggregate data by
    /// <see href="https://docs.openelectricity.org.au/api-reference/market/get-network-data#parameter-interval"/>
    /// </summary>
    public enum DataInterval
    {
        /// <summary>
        /// 5m
        /// </summary>
        [JsonStringEnumMemberName("5m")]
        FiveMinute,

        /// <summary>
        /// 1h
        /// </summary>
        [JsonStringEnumMemberName("1h")]
        OneHour,

        /// <summary>
        /// 7d
        /// </summary>
        [JsonStringEnumMemberName("7d")]
        SevenDay,

        /// <summary>
        /// !M
        /// </summary>
        [JsonStringEnumMemberName("1M")]
        OneMonth,

        /// <summary>
        /// 3M
        /// </summary>
        [JsonStringEnumMemberName("3M")]
        ThreeMonth,

        /// <summary>
        /// season
        /// </summary>
        [JsonStringEnumMemberName("season")]
        Season,

        /// <summary>
        /// 1y
        /// </summary>
        [JsonStringEnumMemberName("1y")]
        OneYear,

        /// <summary>
        /// fy
        /// </summary>
        [JsonStringEnumMemberName("fy")]
        FinancialYear
    }

    /// <summary>
    /// Set of extension methods to allow custom formatting of the <see cref="DataInterval"/> enumeration
    /// </summary>
    public static class DataIntervalExtensions
    {
        /// <summary>
        /// Converts the data interval to the json string equivalent as recognised by the OpenElectricity API.
        /// <see href="https://docs.openelectricity.org.au/api-reference/market/get-network-data#parameter-interval"/>
        /// </summary>
        /// <param name="dataInterval"></param>
        /// <returns></returns>
        public static string ToJsonString(this DataInterval dataInterval)
        {
            Type type = typeof(DataInterval);
            string? name = type.GetEnumName(dataInterval);
            if (name is null) return "";
            return type.GetField(name)?
                .GetCustomAttributes(false)
                .OfType<JsonStringEnumMemberNameAttribute>()
                .SingleOrDefault()?.Name.ToString() ?? "";
        }
    }

    /// <summary>
    /// Primary grouping to apply
    /// </summary>
    public enum DataPrimaryGrouping
    {
        /// <summary>
        /// Group by <see cref="NetworkCode"/>
        /// </summary>
        network,

        /// <summary>
        /// Group by network region
        /// </summary>
        network_region
    }

    /// <summary>
    /// Optional secondary grouping to apply
    /// </summary>
    public enum DataSecondaryGrouping
    {
        /// <summary>
        /// Group by <see cref="UnitFueltechType"/>
        /// </summary>
        fueltech,

        /// <summary>
        /// Group by <see cref="UnitFueltechGroupType"/>
        /// </summary>
        fueltech_group,

        /// <summary>
        /// Group by if <see cref="UnitFueltechType"/> is renewable or not.
        /// <see href="https://docs.openelectricity.org.au/guides/fueltechs#fueltechs"/>
        /// </summary>
        renewable
    }

    /// <summary>
    /// Types of metrics that can be queried
    /// </summary>
    public enum Metric
    {
        /// <summary>
        /// Instantaneous power output/consumption (MW)
        /// </summary>
        power,

        /// <summary>
        /// Energy generated/consumed over time (MWh)
        /// </summary>
        energy,

        /// <summary>
        /// CO2 equivalent emissions (tonnes)
        /// </summary>
        emissions,

        /// <summary>
        /// Total market value ($)
        /// </summary>
        market_value,

        /// <summary>
        /// Price per unit of energy ($/MWh)
        /// </summary>
        price,

        /// <summary>
        /// Demand for energy (MW)
        /// </summary>
        demand,

        /// <summary>
        /// </summary>
        demand_energy
    }

    /// <summary>
    /// Types of metrics that can be queried from generation data
    /// </summary>
    public enum DataMetric
    {
        /// <summary>
        /// Instantaneous power output/consumption (MW)
        /// </summary>
        power = Metric.power,

        /// <summary>
        /// Energy generated/consumed over time (MWh)
        /// </summary>
        energy = Metric.energy,

        /// <summary>
        /// CO2 equivalent emissions (tonnes)
        /// </summary>
        emissions = Metric.emissions,

        /// <summary>
        /// Total market value ($)
        /// </summary>
        market_value = Metric.market_value,
    }

    /// <summary>
    /// Types of metrics that can be queried from market data
    /// </summary>
    public enum MarketMetric
    {
        /// <summary>
        /// Price per unit of energy ($/MWh)
        /// </summary>
        price = Metric.price,

        /// <summary>
        /// Demand for energy (MW)
        /// </summary>
        demand = Metric.demand,

        /// <summary>
        /// </summary>
        demand_energy = Metric.demand_energy,
    }

    /// <summary>
    /// Status of unit in facility
    /// </summary>
    public enum UnitStatusType
    {
        /// <summary>
        /// Planned facility
        /// </summary>
        announced,

        /// <summary>
        /// Being tested
        /// </summary>
        commissioning,

        /// <summary>
        /// Currently generating
        /// </summary>
        operating,

        /// <summary>
        /// Temporarily closed
        /// </summary>
        mothballed,

        /// <summary>
        /// Permanently closed
        /// </summary>
        retired,
    }

    /// <summary>
    /// Individual generation technologies
    /// </summary>
    public enum UnitFueltechType
    {
        /// <summary>
        /// Battery. Generic battery category (distinct from charging/discharging)
        /// </summary>
        battery,

        /// <summary>
        /// Battery (Charging)
        /// </summary>
        battery_charging,

        /// <summary>
        /// Battery (Discharging)
        /// </summary>
        battery_discharging,

        /// <summary>
        /// Biogas
        /// </summary>
        bioenergy_biogas,

        /// <summary>
        /// Biomass
        /// </summary>
        bioenergy_biomass,

        /// <summary>
        /// Coal (Black)
        /// </summary>
        coal_black,

        /// <summary>
        /// Coal (Brown)
        /// </summary>
        coal_brown,

        /// <summary>
        /// Distillate
        /// </summary>
        distillate,

        /// <summary>
        /// Gas (CCGT)
        /// </summary>
        gas_ccgt,

        /// <summary>
        /// Gas (OCGT)
        /// </summary>
        gas_ocgt,

        /// <summary>
        /// Gas (Reciprocating)
        /// </summary>
        gas_recip,

        /// <summary>
        /// Gas (Steam)
        /// </summary>
        gas_steam,

        /// <summary>
        /// Gas (Coal Mine Waste)
        /// </summary>
        gas_wcmg,

        /// <summary>
        /// Hydro
        /// </summary>
        hydro,

        /// <summary>
        /// Pumps
        /// </summary>
        pumps,

        /// <summary>
        /// Solar (Rooftop)
        /// </summary>
        solar_rooftop,

        /// <summary>
        /// Solar (Thermal)
        /// </summary>
        solar_thermal,

        /// <summary>
        /// Solar (Utility)
        /// </summary>
        solar_utility,

        /// <summary>
        /// Nuclear. Currently not used in Australia but maintained for international compatibility
        /// </summary>
        nuclear,

        /// <summary>
        /// Wind
        /// </summary>
        wind,

        /// <summary>
        /// Offshore Wind
        /// </summary>
        wind_offshore,

        /// <summary>
        /// Network Import
        /// </summary>
        imports,

        /// <summary>
        /// Network Export
        /// </summary>
        exports,

        /// <summary>
        /// Interconnector
        /// </summary>
        interconnector,

        /// <summary>
        /// Aggregator (VPP)
        /// </summary>
        aggregator_vpp,

        /// <summary>
        /// Aggregator (DR)
        /// </summary>
        aggregator_dr
    }

    /// <summary>
    /// Broader categories that group similar <see cref="UnitFueltechType"/> technologies 
    /// </summary>
    public enum UnitFueltechGroupType
    {
        /// <summary>
        /// Coal
        /// </summary>
        coal,

        /// <summary>
        /// Gas
        /// </summary>
        gas,

        /// <summary>
        /// Wind
        /// </summary>
        wind,

        /// <summary>
        /// Solar
        /// </summary>
        solar,

        /// <summary>
        /// Battery (Charging)
        /// </summary>
        battery_charging,

        /// <summary>
        /// Battery (Discharging)
        /// </summary>
        battery_discharging,

        /// <summary>
        /// Hydro
        /// </summary>
        hydro,

        /// <summary>
        /// Distillate
        /// </summary>
        distillate,

        /// <summary>
        /// Bioenergy
        /// </summary>
        bioenergy,

        /// <summary>
        /// Pumps
        /// </summary>
        pumps
    }

    /// <summary>
    /// Broad categories that describe the facility role in the electrical network
    /// </summary>
    public enum UnitDispatchType
    {
        /// <summary>
        /// Produces energy
        /// </summary>
        GENERATOR,

        /// <summary>
        /// Consumes energy
        /// </summary>
        LOAD,

        /// <summary>
        /// Distributes energy within an electrical network
        /// </summary>
        NETWORK,

        /// <summary>
        /// Distributes energy between electrical networks
        /// </summary>
        INTERCONNECTOR
    }

    /// <summary>
    /// Types of plans offered by OpenElectricity for connecting to the API
    /// </summary>
    public enum UserPlan
    {
        /// <summary>
        /// Basic
        /// </summary>
        BASIC,

        /// <summary>
        /// Professional
        /// </summary>
        PRO,

        /// <summary>
        /// Enterprise
        /// </summary>
        ENTERPRISE
    }
}
