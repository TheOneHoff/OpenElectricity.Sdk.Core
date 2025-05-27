using Microsoft.Extensions.Options;
using OpenElectricity.Sdk.Helpers;
using OpenElectricity.Sdk.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace OpenElectricity.Sdk
{
    /// <summary>
    /// A wrapper over the OpenElectricity API for .NET Core.
    /// For more information about the API, visit <see href="https://docs.openelectricity.org.au/api-reference/overview"/>
    /// </summary>
    public class OpenElectricityClient
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions = StaticJsonSerializerOptions.GetDefaultOptions();

        const string DateTimeFormat = "s";

        bool hasfirstRequestFinished = false;
        readonly SemaphoreSlim _semaphore = new(1);

        /// <summary>
        /// Create an <see cref="OpenElectricityClient" /> with default settings
        /// </summary>
        /// <param name="options"></param>
        public OpenElectricityClient(OpenElectricityOptions options)
        {
            var handler = HttpClientHelpers.GetDefaultMessageHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.ConfigureDefaultHttpClient(options);
        }

        /// <summary>
        /// Create an <see cref="OpenElectricityClient" /> using dependency injection
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options"></param>
        public OpenElectricityClient(HttpClient httpClient, IOptions<OpenElectricityOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.ConfigureDefaultHttpClient(options.Value);
        }

        private async Task<T> SendAsync<T>(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            if (!hasfirstRequestFinished)
            {
                await _semaphore.WaitAsync(cancellationToken);
                if (hasfirstRequestFinished)
                {
                    _semaphore.Release();
                }
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);
            APIResponse<T> result;
            try
            {
                if (response.StatusCode == HttpStatusCode.UnprocessableContent)
                {
                    Error422Response error = await response.Content.ReadFromJsonAsync<Error422Response>(_serializerOptions, cancellationToken)
                        ?? throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");

                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Reason: {error.Detail?.Msg}");
                }

                result = await response.Content.ReadFromJsonAsync<APIResponse<T>>(_serializerOptions, cancellationToken)
                    ?? throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");

                if (!result.Success)
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Reason: {result.Error}");
                }
                if (result.Data is null)
                {
                    throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");
                }

                if (!hasfirstRequestFinished)
                {
                    hasfirstRequestFinished = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled exception when executing {request.RequestUri}", ex);
            }
            finally
            {
                _semaphore.Release();
            }

            return result.Data;
        }

        /// <summary>
        /// Get the current user.
        /// <see href="https://docs.openelectricity.org.au/api-reference/user/get-user-me"/>
        /// </summary>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<User> GetUserAsync(
            bool withClerk = true,
            CancellationToken cancellationToken = default
            )
        {
            string route = "me";

            UriQueryParams parameters = new();
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"{route}{parameters}");

            return await SendAsync<User>(request, cancellationToken);
        }

        /// <summary>
        /// Get all approved facilities and their associated units.
        /// <see href="https://docs.openelectricity.org.au/api-reference/facilities/get-facilities"/>
        /// </summary>
        /// <param name="facilityCode">Filter by facility code(s)</param>
        /// <param name="statusId">Filter by unit status(es)</param>
        /// <param name="fueltechId">Filter by unit fuel technology type(s)</param>
        /// <param name="networkId">Filter by network code(s)</param>
        /// <param name="networkRegion">Filter by network region</param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<List<Facility>> GetFacilitiesAsync(
            List<string>? facilityCode = null,
            List<UnitStatusType>? statusId = null,
            List<UnitFueltechType>? fueltechId = null,
            List<string>? networkId = null,
            string? networkRegion = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default
            )
        {
            string route = "facilities/";

            UriQueryParams parameters = new();
            parameters.Add("facility_code", facilityCode ?? []);
            parameters.Add("status_id", statusId?.Select(s => s.ToString()) ?? []);
            parameters.Add("fueltech_id", fueltechId?.Select(s => s.ToString()) ?? []);
            parameters.Add("network_id", networkId ?? []);
            parameters.Add("network_region", networkRegion);
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"{route}{parameters}");

            return await SendAsync<List<Facility>>(request, cancellationToken);
        }

        /// <summary>
        /// Get market data for a network. 
        /// <see href="https://docs.openelectricity.org.au/api-reference/market/get-network-data"/>
        /// </summary>
        /// <param name="networkCode"></param>
        /// <param name="metrics"></param>
        /// <param name="interval"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="primaryGrouping"></param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<List<NetworkData>> GetMarketDataAsync(
            NetworkCode networkCode,
            List<MarketMetric> metrics,
            DataInterval interval,
            DateTime? dateStart = null,
            DateTime? dateEnd = null,
            DataPrimaryGrouping? primaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            string route = $"market/network/{networkCode}";

            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"{route}{parameters}");

            return await SendAsync<List<NetworkData>>(request, cancellationToken);
        }

        /// <summary>
        /// Get time series data for a network.
        /// <see href="https://docs.openelectricity.org.au/api-reference/data/get-network-data"/>
        /// </summary>
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
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<List<NetworkData>> GetGenerationDataAsync(
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            DateTime? dateStart = null,
            DateTime? dateEnd = null,
            DataPrimaryGrouping? primaryGrouping = null,
            DataSecondaryGrouping? secondaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            string route = $"data/network/{networkCode}";

            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("secondary_grouping", secondaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"{route}{parameters}");

            return await SendAsync<List<NetworkData>>(request, cancellationToken);

        }

        /// <summary>
        /// Get time series data for a specific facility.
        /// <see href="https://docs.openelectricity.org.au/api-reference/data/get-facility-data"/>
        /// </summary>
        /// <param name="networkCode"></param>
        /// <param name="metrics"></param>
        /// <param name="interval"></param>
        /// <param name="facilityCodes"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<List<NetworkData>> GetFacilityDataAsync(
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            List<string>? facilityCodes = null,
            DateTime? dateStart = null,
            DateTime? dateEnd = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            string route = $"data/facilities/{networkCode}";

            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("facility_code", facilityCodes ?? []);
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"{route}{parameters}");

            return await SendAsync<List<NetworkData>>(request, cancellationToken);
        }
    }
}
