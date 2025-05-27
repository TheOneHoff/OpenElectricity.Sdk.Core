namespace OpenElectricity.Sdk
{
    /// <summary>
    /// Used to configure the OpenElectricityClient
    /// </summary>
    public class OpenElectricityOptions
    {
        /// <summary>
        /// The api key aquired from <see href="https://platform.openelectricity.org.au/dashboard/keys" />
        /// </summary>
        public required string ApiKey { get; set; }

        /// <summary>
        /// The base url of the OpenElectricity API
        /// </summary>
        /// <value>Default: <see href="https://api.openelectricity.org.au/v4/" /></value>
        public Uri BaseUrl { get; set; } = new("https://api.openelectricity.org.au/v4/");
    }
}
