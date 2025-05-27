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
            builder.Services.AddScoped<HostedTesting>();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var testing = scope.ServiceProvider.GetService<HostedTesting>();
            if (testing is null) return;

            await testing.RunAsync();
            return;
        }
    }
}
