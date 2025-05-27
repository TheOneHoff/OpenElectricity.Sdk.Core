using Microsoft.Extensions.Options;
using OpenElectricity.Sdk.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace OpenElectricity.Sdk
{
    public class OpenElectricityClient
    {
        readonly HttpClient _httpClient;
        readonly string _apiKey;
        readonly JsonSerializerOptions _serializerOptions;

        const string DateTimeFormat = "s";

        /// <summary>
        /// Create an OpenElectricityClient with default settings
        /// </summary>
        /// <param name="options"></param>
        public OpenElectricityClient(OpenElectricityOptions options)
        {
            _httpClient = new()
            {
                BaseAddress = options.BaseUrl,
            };
            _apiKey = options.ApiKey;
            
            _serializerOptions = new StaticJsonSerializerOptions().Default;
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
            _apiKey = options.Value.ApiKey;
            _serializerOptions = new StaticJsonSerializerOptions().Default;
        }

        private async Task<T> SendAsync<T>(
            HttpMethod method,
            string relativePath,
            UriQueryParams queryParams,
            CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new(method, $"{relativePath}{queryParams}");
            request.Headers.Authorization = new("Bearer", _apiKey);

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
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled exception when executing {request.RequestUri}", ex);
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
        public async Task<User> GetUserAsync(
            bool withClerk = true,
            CancellationToken cancellationToken = default
            )
        {
            UriQueryParams parameters = new();
            parameters.Add("with_clerk", withClerk.ToString());

            return await SendAsync<User>(HttpMethod.Get, "me", parameters, cancellationToken);
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

            return await SendAsync<List<Facility>>(HttpMethod.Get, "facilities/", parameters, cancellationToken);
        }

        public async Task<List<NetworkData>> GetMarketDataAsync(
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
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            return await SendAsync<List<NetworkData>>(HttpMethod.Get, $"market/network/{networkCode}", parameters, cancellationToken);
        }

        public async Task<List<NetworkData>> GetGenerationDataAsync(
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
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("primary_grouping", primaryGrouping.ToString());
            parameters.Add("secondary_grouping", secondaryGrouping.ToString());
            parameters.Add("with_clerk", withClerk.ToString());

            return await SendAsync<List<NetworkData>>(HttpMethod.Get, $"data/network/{networkCode}", parameters, cancellationToken);

        }
        public async Task<List<NetworkData>> GetFacilityDataAsync(
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
            parameters.Add("date_start", dateStart?.ToString(DateTimeFormat));
            parameters.Add("date_end", dateEnd?.ToString(DateTimeFormat));
            parameters.Add("with_clerk", withClerk.ToString());

            return await SendAsync<List<NetworkData>>(HttpMethod.Get, $"data/facilities/{networkCode}", parameters, cancellationToken);
        }
    }
}
