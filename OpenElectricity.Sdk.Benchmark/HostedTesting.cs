using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenElectricity.Sdk.Models;
using System.Text.Json;

namespace OpenElectricity.Sdk.Benchmark
{
    internal class HostedTesting(
        ILogger<HostedTesting> logger,
        OpenElectricityClient openElectricityClient) : IHostedService
    {
        readonly OpenElectricityClient _client = openElectricityClient;
        readonly ILogger<HostedTesting> _logger = logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var user = await _client.GetUserAsync(cancellationToken: cancellationToken);
                _logger.LogDebug("GetUser success. Result: {userDto}", JsonSerializer.Serialize(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUser failure");
            }

            try
            {
                var facilities = await _client.GetFacilitiesAsync(cancellationToken: cancellationToken);
                _logger.LogDebug("GetFacilities success. Result: {userDto}", JsonSerializer.Serialize(facilities));
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
                _logger.LogDebug("GetFacilityData success. Result: {userDto}", JsonSerializer.Serialize(facility_data));
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
                _logger.LogDebug("GetMarketData success. Result: {userDto}", JsonSerializer.Serialize(market_data));
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
                _logger.LogDebug("GetGenerationData success. Result: {userDto}", JsonSerializer.Serialize(generation_data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGenerationData failure");
            }

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
