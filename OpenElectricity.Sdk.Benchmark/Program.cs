using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenElectricity.Sdk.Benchmark
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.Configure<OpenElectricityOptions>(builder.Configuration.GetSection(nameof(OpenElectricityOptions)));
            builder.Services.AddHttpClient<OpenElectricityClient>();
            builder.Services.AddHostedService<HostedTesting>();

            var app = builder.Build();
            
            await app.RunAsync();
            return;
        }
    }
}
