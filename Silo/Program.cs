using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = new HostBuilder()
    .UseOrleans(siloBuilder =>
    {
        siloBuilder
            .AddAdoNetGrainStorage("robotStateStore", options =>
            {
                options.ConnectionString = "Server=localhost;Database=LearningOrleans;User ID=batman;Password=1sAGrumpyFella;Trusted_Connection=False;Encrypt=False;MultipleActiveResultSets=True;";
            })
            .UseLocalhostClustering()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .UseDashboard();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();