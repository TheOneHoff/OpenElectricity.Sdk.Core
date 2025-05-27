using Microsoft.Extensions.Options;
using OpenElectricity.Sdk.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace OpenElectricity.Sdk
{
    public class OpenElectricityClient
    {
        readonly HttpClient _httpClient;

        readonly JsonSerializerOptions _serializerOptions;

        /// <summary>
        /// Create an OpenElectricityClient with default settings
        /// </summary>
        /// <param name="options"></param>
        public OpenElectricityClient(OpenElectricityOptions options)
        {
            _httpClient = new()
            {
                BaseAddress = options.BaseUrl
            };
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", options.ApiKey);
            _serializerOptions = options.SerializerOptions;
        }

        /// <summary>
        /// Create an OpenElectricityClient using dependency injection
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options"></param>
        public OpenElectricityClient(HttpClient httpClient, IOptions<OpenElectricityOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = options.Value.BaseUrl;
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", options.Value.ApiKey);
            _serializerOptions = options.Value.SerializerOptions;
        }

        private async Task<T> SendAsync<T>(
            HttpRequestMessage request,
            UriQueryParams parameters,
            CancellationToken cancellationToken)
        {
            request.RequestUri = new($"{request.RequestUri!.PathAndQuery}{parameters}");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableContent)
            {
                Error422Response error = await response.Content.ReadFromJsonAsync<Error422Response>(_serializerOptions, cancellationToken)
                    ?? throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");

                throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Reason: {error.Detail?.Msg}");
            }

            APIResponse<T> result = await response.Content.ReadFromJsonAsync<APIResponse<T>>(_serializerOptions, cancellationToken)
                ?? throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");

            if (!result.Success)
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Reason: {result.Error}");
            }
            if (result.Data is null)
            {
                throw new JsonException($"Unable to deserialize response with status code {response.StatusCode}");
            }

            return result.Data;
        }

        /// <summary>
        /// Get the current user
        /// </summary>
        /// <param name="withClerk"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">If there is an error during the request, or if the request returns an error status code</exception>
        /// <exception cref="JsonException">If there is an error when deserializing</exception>
        public async Task<UserDto> GetUserAsync(
            bool withClerk = true,
            CancellationToken cancellationToken = default
            )
        {
            UriQueryParams parameters = new();
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, "me");
            return await SendAsync<UserDto>(request, parameters, cancellationToken);
        }

        /// <summary>
        /// Get all approved facilities and their associated units
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
            UriQueryParams parameters = new();
            parameters.Add("facility_code", facilityCode ?? []);
            parameters.Add("status_id", statusId?.Select(s => s.ToString()) ?? []);
            parameters.Add("fueltech_id", fueltechId?.Select(s => s.ToString()) ?? []);
            parameters.Add("network_id", networkId ?? []);
            parameters.Add("network_region", networkRegion);
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, "facilities");
            return await SendAsync<List<Facility>>(request, parameters, cancellationToken);
        }

        public async Task<NetworkData> GetMarketDataAsync(
            NetworkCode networkCode,
            List<MarketMetric> metrics,
            DataInterval interval,
            DateTimeOffset? dateStart = null,
            DateTimeOffset? dateEnd = null,
            DataPrimaryGrouping? primaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("date_start", dateStart?.ToString("u"));
            parameters.Add("date_end", dateEnd?.ToString("u"));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"market/network/{networkCode}");
            return await SendAsync<NetworkData>(request, parameters, cancellationToken);
        }

        public async Task<NetworkData> GetGenerationDataAsync(
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            DateTimeOffset? dateStart = null,
            DateTimeOffset? dateEnd = null,
            DataPrimaryGrouping? primaryGrouping = null,
            DataSecondaryGrouping? secondaryGrouping = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("date_start", dateStart?.ToString("u"));
            parameters.Add("date_end", dateEnd?.ToString("u"));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("secondary_grouping", secondaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"data/network/{networkCode}");
            return await SendAsync<NetworkData>(request, parameters, cancellationToken);

        }
        public async Task<NetworkData> GetFacilityDataAsync(
            NetworkCode networkCode,
            List<DataMetric> metrics,
            DataInterval interval,
            List<string>? facilityCodes = null,
            DateTimeOffset? dateStart = null,
            DateTimeOffset? dateEnd = null,
            bool withClerk = true,
            CancellationToken cancellationToken = default)
        {
            UriQueryParams parameters = new();
            parameters.Add("metrics", metrics.Select(m => m.ToString()));
            parameters.Add("interval", interval.ToJsonString());
            parameters.Add("facility_code", facilityCodes ?? []);
            parameters.Add("date_start", dateStart?.ToString("u"));
            parameters.Add("date_end", dateEnd?.ToString("u"));
            parameters.Add("with_clerk", withClerk.ToString());

            HttpRequestMessage request = new(HttpMethod.Get, $"data/facilities/{networkCode}");
            return await SendAsync<NetworkData>(request, parameters, cancellationToken);
        }
    }
}
