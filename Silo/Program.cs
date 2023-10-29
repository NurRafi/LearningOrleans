using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = new HostBuilder()
    .UseOrleans(siloBuilder =>
    {
        siloBuilder
            .UseLocalhostClustering()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            });
    })
    .UseConsoleLifetime();

using var host = hostBuilder.Build();
await host.RunAsync();