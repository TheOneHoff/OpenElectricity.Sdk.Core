using Microsoft.Extensions.Logging;
using OpenElectricity.Sdk.Models;

namespace OpenElectricity.Sdk.Benchmark
{
    internal class HostedTesting(
        ILogger<HostedTesting> logger,
        OpenElectricityClient openElectricityClient)
    {
        readonly OpenElectricityClient _client = openElectricityClient;
        readonly ILogger<HostedTesting> _logger = logger;

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _client.GetUserAsync(cancellationToken: cancellationToken);
                _logger.LogDebug("GetUser success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUser failure");
            }

            try
            {
                var facilities = await _client.GetFacilitiesAsync(cancellationToken: cancellationToken);
                _logger.LogDebug("GetFacilities success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFacilities failure");
            }

            try
            {
                var facility_data = await _client.GetFacilityDataAsync(
                    networkCode: NetworkCode.NEM,
                    metrics: [DataMetric.energy, DataMetric.power],
                    interval: DataInterval.FiveMinute,
                    dateStart: DateTimeOffset.Now.AddDays(-1),
                    dateEnd: DateTimeOffset.Now,
                    cancellationToken: cancellationToken);
                _logger.LogDebug("GetFacilityData success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFacilityData failure");
            }


            try
            {
                var market_data = await _client.GetMarketDataAsync(
                    networkCode: NetworkCode.NEM,
                    metrics: [MarketMetric.price, MarketMetric.demand],
                    interval: DataInterval.FiveMinute,
                    dateStart: DateTimeOffset.Now.AddDays(-1),
                    dateEnd: DateTimeOffset.Now,
                    cancellationToken: cancellationToken);
                _logger.LogDebug("GetMarketData success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMarketData failure");
            }


            try
            {
                var generation_data = await _client.GetGenerationDataAsync(
                    networkCode: NetworkCode.NEM,
                    metrics: [DataMetric.energy, DataMetric.power],
                    interval: DataInterval.FiveMinute,
                    dateStart: DateTimeOffset.Now.AddDays(-1),
                    dateEnd: DateTimeOffset.Now,
                    cancellationToken: cancellationToken);
                _logger.LogDebug("GetGenerationData success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGenerationData failure");
            }

            return;
        }
    }
}
