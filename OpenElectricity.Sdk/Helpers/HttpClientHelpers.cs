using OpenElectricity.Sdk.Client;

namespace OpenElectricity.Sdk.Helpers
{
    internal static class HttpClientHelpers
    {
        const string Bearer = "Bearer";

        public static HttpMessageHandler GetDefaultMessageHandler()
        {
            return new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new()
            };
        }

        public static HttpClient ConfigureDefaultHttpClient(this HttpClient client, OpenElectricityOptions options)
        {
            client.BaseAddress = options.BaseUrl;
            client.DefaultRequestHeaders.Authorization = new(Bearer, options.ApiKey);
            return client;
        }
    }
}
