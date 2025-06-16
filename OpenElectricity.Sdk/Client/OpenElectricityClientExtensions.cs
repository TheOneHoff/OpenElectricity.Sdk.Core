using OpenElectricity.Sdk.Types;

namespace OpenElectricity.Sdk.Client
{
    /// <summary>
    /// Set of helper extensions for fetching and processing data using the <see cref="OpenElectricityClient"/>
    /// </summary>
    public static class OpenElectricityClientExtensions
    {
        /// <summary>
        /// Get market data for a network. Sends multiple requests if the date range is too large for the interval
        /// </summary>
        /// <param name="client"></param>
        /// <param name="networkCode"></param>
        /// <param name="metrics"></param>
        /// <param name="interval"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="primaryGrouping"></param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<NetworkData>> GetMarketDataForDateRangeAsync(
            this OpenElectricityClient client,
            NetworkCode networkCode,
            List<MarketMetric> metrics,
            DataInterval interval,
            DateTime dateStart,
            DateTime dateEnd,
            DataPrimaryGrouping? primaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default

            )
        {
            return await GetAllForDateRangeAsync((inter, start, end, token) =>
            {
                return client.GetMarketDataAsync(
                    networkCode, metrics, inter, start, end,
                    primaryGrouping, withClerk,
                    token);
            },
            interval, dateStart, dateEnd, cancellationToken);
        }

        /// <summary>
        /// Get time series data for a network. Sends multiple requests if the date range is too large for the interval
        /// </summary>
        /// <param name="client"></param>
        /// <param name="networkCode"></param>
        /// <param name="metrics"></param>
        /// <param name="interval"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="primaryGrouping"></param>
        /// <param name="secondaryGrouping"></param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<NetworkData>> GetGenerationDataForDateRangeAsync(
            this OpenElectricityClient client,
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            DateTime dateStart,
            DateTime dateEnd,
            DataPrimaryGrouping? primaryGrouping = null,
            DataSecondaryGrouping? secondaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default

            )
        {
            return await GetAllForDateRangeAsync((inter, start, end, token) =>
            {
                return client.GetGenerationDataAsync(
                    networkCode, metrics, inter, start, end,
                    primaryGrouping, secondaryGrouping, withClerk,
                    token);
            },
            interval, dateStart, dateEnd, cancellationToken);
        }

        /// <summary>
        /// Get time series data for a specific facility. Sends multiple requests if the date range is too large for the interval
        /// </summary>
        /// <param name="client"></param>
        /// <param name="networkCode"></param>
        /// <param name="metrics"></param>
        /// <param name="interval"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="facilityCodes"></param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<List<NetworkData>> GetFacilityDataForDateRangeAsync(
            this OpenElectricityClient client,
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            DateTime dateStart,
            DateTime dateEnd,
            List<string>? facilityCodes = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default

            )
        {
            return await GetAllForDateRangeAsync((inter, start, end, token) =>
            {
                return client.GetFacilityDataAsync(
                    networkCode, metrics, inter, facilityCodes, 
                    start, end, withClerk,
                    token);
            },
            interval, dateStart, dateEnd, cancellationToken);
        }

        /// <summary>
        /// Helper function to fetch all data for the provided <see cref="DataInterval"/> between two timestamps.
        /// </summary>
        /// <param name="getDataFunction"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<NetworkData>> GetAllForDateRangeAsync(
            Func<DataInterval, DateTime, DateTime, CancellationToken, Task<List<NetworkData>>> getDataFunction,
            DataInterval interval,
            DateTime start,
            DateTime end,
            CancellationToken cancellationToken = default
            )
        {
            int dayRange = interval.DayRange() ?? throw new Exception($"Day range for interval {interval} is invalid");
            if (dayRange <= 0)
            {
                throw new Exception($"Day range of [{dayRange}] for interval {interval} is invalid");
            }

            List<Task<List<NetworkData>>> tasks = [];
            DateTime currentStart = start;
            while (currentStart < end)
            {
                DateTime currentEnd = currentStart.AddDays(dayRange);
                if (currentEnd > end)
                {
                    currentEnd = end;
                }

                Task<List<NetworkData>> task = getDataFunction(interval, currentStart, currentEnd, cancellationToken);
                tasks.Add(task);
                currentStart = currentEnd;
            }

            List<NetworkData>[]? results = await Task.WhenAll(tasks);
            return [.. results.SelectMany(r => r)];
        }
    }
}
