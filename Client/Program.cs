using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GrainInterfaces;

using var host = new HostBuilder()
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var robot = client.GetGrain<IRobot>("Nur");

await robot.AddInstruction("Go left");
await robot.AddInstruction("Go right");
Console.WriteLine(await robot.GetInstructionCount());
Console.WriteLine(await robot.GetNextInstruction());
Console.WriteLine(await robot.GetInstructionCount());
await robot.AddInstruction("Pick item");

await host.StopAsync();