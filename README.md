# OpenElectricity.Sdk.Core
.NET Core client library for the [OpenElectricity](https://openelectricity.org.au) API

Nuget package is available at [nuget.org/packages/OpenElectricity.Sdk.Core](https://www.nuget.org/packages/OpenElectricity.Sdk.Core)

> [!NOTE]
> This project is not affiliated with OpenElectricity

> [!WARNING]
> The v4 OpenElectricity API is currently under active development.

## Documentation
To obtain an API key visit [platform.openelectricity.org.au](https://platfrom.openelectricity.org.au)

For documentation visit [docs.openelectricity.org.au](https://docs.openelectricity.org.au/introduction)

The officially supported Python client is available at [github.com/openelectricity/openelectricity-python](https://github.com/opennem/openelectricity-python)

The officially supported Typescript client is available at [github.com/openelectricity/openelectricity-typescript](https://github.com/opennem/openelectricity-typescript)

## Features
- Full C# support for all publicly available HTTP routes
- Comprehensive class and enum definitions
- Full async support for requests
- Dependency injection support

## Installation
**Dotnet CLI**
```bash
dotnet add package OpenElectricity.Sdk.Core
```

## Quick Start
Generate an API key here [platform.openelectricity.org.au](https://platfrom.openelectricity.org.au)

### Basic Usage
> [!NOTE]
> Only one client should be created per application to avoid port exhaustion [learn.microsoft.com/httpclient-guidelines](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines)

Create a new client and pass in your API key
```C#
using OpenElectricity.Sdk.Client;

namespace NewProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Pass your API key into the options
            var options = new OpenElectricityOptions()
            {
                ApiKey = "[YOUR API KEY HERE]"
            };

            // Initialize the client
            var client = new OpenElectricityClient(options);

            // Use the client
            var facilities = await client.GetFacilitiesAsync();
        }
    }
}
```
### Dependency injection
Create an appsettings.json with the following configuration
```JSON
{
  "OpenElectricityOptions": {
    "ApiKey": "[YOUR API KEY HERE]",
  }
}
```

Create your class that uses the OpenElectricityClient
```C#
using OpenElectricity.Sdk.Client;

namespace NewProject
{
    public class NewClass(OpenElectricityClient openElectricityClient)
    {
        readonly OpenElectricityClient _client = openElectricityClient;

        public async Task DoSomethingAsync(CancellationToken cancellationToken = default)
        {
            var facilities = await _client.GetFacilitiesAsync(cancellationToken: cancellationToken);
        }
    }
}
```

Setup dependency injection in Program.cs
```C#
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenElectricity.Sdk.Client;

namespace NewProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Create the DI builder
            var builder = Host.CreateApplicationBuilder(args);

            // Register the client in DI
            builder.Services.UseOpenElectricityClient(builder.Configuration);

            // Register your class in DI
            builder.Services.AddScoped<NewClass>();

            // Build the application
            var app = builder.Build();

            // Use your class
            var scope = app.Services.CreateScope();
            var newClass = scope.ServiceProvider.GetService<NewClass>();
            if (newClass is null) return;

            await newClass.DoSomethingAsync();
            return;
        }
    }
}
```

## Development
```bash
# Install dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# NuGet pack
dotnet pack
```

## License
MIT