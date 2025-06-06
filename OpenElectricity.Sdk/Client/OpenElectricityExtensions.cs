using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenElectricity.Sdk.Helpers;

namespace OpenElectricity.Sdk.Client
{
    /// <summary>
    /// Set of extension methods providing easier initialization using dependency injection
    /// </summary>
    public static class OpenElectricityExtensions
    {
        /// <summary>
        /// Adds OpenElectricityClient services to the specified <see cref="IServiceCollection" />
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns>The <see cref="IHttpClientBuilder" /> so that additional calls can be chained</returns>
        public static IHttpClientBuilder UseOpenElectricityClient(
            this IServiceCollection services, 
            Action<OpenElectricityOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.RegisterOpenElectricityClient();
        }

        /// <summary>
        /// Adds OpenElectricityClient services to the specified <see cref="IServiceCollection" />. 
        /// Automatically reads from the <see cref="OpenElectricityOptions" /> section of the <see cref="IConfiguration" />
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>The <see cref="IHttpClientBuilder" /> so that additional calls can be chained</returns>
        public static IHttpClientBuilder UseOpenElectricityClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OpenElectricityOptions>(configuration.GetSection(nameof(OpenElectricityOptions)));
            return services.RegisterOpenElectricityClient();
        }

        private static IHttpClientBuilder RegisterOpenElectricityClient(this IServiceCollection services)
        {
            return services
                .AddHttpClient<OpenElectricityClient>()
                .ConfigurePrimaryHttpMessageHandler(HttpClientHelpers.GetDefaultMessageHandler);
        }
    }
}
