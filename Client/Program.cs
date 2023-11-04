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

while (true)
{
    Console.WriteLine("Robot name: ");
    var grain = client.GetGrain<IRobotGrain>(Console.ReadLine());

    Console.WriteLine("Instruction: ");
    await grain.AddInstruction(Console.ReadLine());

    Console.WriteLine($"{grain.GetGrainId()} has {await grain.GetInstructionCount()} instructions.");
}

// await host.StopAsync();