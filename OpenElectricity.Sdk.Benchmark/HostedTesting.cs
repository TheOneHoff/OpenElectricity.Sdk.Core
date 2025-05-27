using Microsoft.Extensions.Logging;
using OpenElectricity.Sdk.Models;
using System.Text;
using System.Threading;

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
                    dateStart: DateTime.Now.AddDays(-1),
                    dateEnd: DateTime.Now,
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
                    dateStart: DateTime.Now.AddDays(-1),
                    dateEnd: DateTime.Now,
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
                    dateStart: DateTime.Now.AddDays(-1),
                    dateEnd: DateTime.Now,
                    cancellationToken: cancellationToken);
                _logger.LogDebug("GetGenerationData success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGenerationData failure");
            }

            return;
        }

        public async Task RunCarbonDataFetchAsync(
            DateTime start,
            DateTime end,
            string folderPath,
            CancellationToken cancellationToken = default)
        {
            NetworkCode networkCode = NetworkCode.NEM;
            DataMetric metric = DataMetric.energy;

            DateTime currentStart = start;
            while(currentStart < end)
            {
                DateTime currentEnd = currentStart.AddDays(30);
                if (currentEnd > end)
                {
                    currentEnd = end;
                }

                await Fetch30Days(folderPath, networkCode, metric, currentStart, currentEnd, cancellationToken);

                currentStart = currentStart.AddDays(30);
            }

            return;
        }

        private async Task<bool> Fetch30Days(
            string folderPath,
            NetworkCode networkCode, 
            DataMetric metric, 
            DateTime start, 
            DateTime end,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var market_data = await _client.GetGenerationDataAsync(
                    networkCode: networkCode,
                    metrics: [metric],
                    interval: DataInterval.OneHour,
                    dateStart: start,
                    dateEnd: end,
                    cancellationToken: cancellationToken);

                var emissions = market_data.FirstOrDefault(r => r.Metric == (Metric)metric)?.Results.FirstOrDefault()?.TimeSeries ?? [];

                string outputFile = $"{networkCode}.{metric}_{start:yyyyMMddHHmmss}-{end:yyyyMMddHHmmss}.csv";

                string filePath = Path.Combine(folderPath, outputFile);
                FileInfo file = new(filePath);
                if (file.Directory is not null && !file.Directory.Exists)
                {
                    Directory.CreateDirectory(file.Directory.FullName);
                }
                if (file.Exists)
                {
                    File.Delete(file.FullName);
                }

                using FileStream fileStream = new(file.FullName, FileMode.Create, FileAccess.ReadWrite);
                using StreamWriter streamWriter = new(fileStream, Encoding.UTF8);

                string header = "Region, DateTime, Value";
                streamWriter.WriteLine(header);

                foreach (var tv in emissions)
                {
                    streamWriter.WriteLine($"{tv.Timestamp:s}, {tv.Value}");
                }

                _logger.LogDebug("GetMarketData success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMarketData failure");
                return false;
            }
            return true;
        }
    }
}
