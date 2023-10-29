using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GrainInterfaces;

var hostBuilder = new HostBuilder()
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

using var host = hostBuilder.Build();
await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var robot = client.GetGrain<IRobot>("Nur");

await robot.AddInstruction("Go Left");
await robot.AddInstruction("Go Right");
Console.WriteLine(await robot.GetInstructionCount());
Console.WriteLine(await robot.GetNextInstruction());
Console.WriteLine(await robot.GetInstructionCount());

await host.StopAsync();