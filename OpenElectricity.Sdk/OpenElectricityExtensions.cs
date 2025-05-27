using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenElectricity.Sdk.Helpers;

namespace OpenElectricity.Sdk
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
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained</returns>
        public static IServiceCollection UseOpenElectricityClient(
            this IServiceCollection services, 
            Action<OpenElectricityOptions> setupAction)
        {
            services.Configure(setupAction);
            services.RegisterOpenElectricityClient();
            return services;
        }

        /// <summary>
        /// Adds OpenElectricityClient services to the specified <see cref="IServiceCollection" />. 
        /// Automatically reads from the <see cref="OpenElectricityOptions" /> section of the <see cref="IConfiguration" />
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained</returns>
        public static IServiceCollection UseOpenElectricityClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OpenElectricityOptions>(configuration.GetSection(nameof(OpenElectricityOptions)));
            services.RegisterOpenElectricityClient();
            return services;
        }

        private static void RegisterOpenElectricityClient(this IServiceCollection services)
        {
            services
                .AddHttpClient<OpenElectricityClient>()
                .ConfigurePrimaryHttpMessageHandler(HttpClientHelpers.GetDefaultMessageHandler);
        }
    }
}
