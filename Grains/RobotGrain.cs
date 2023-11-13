using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Grains;

[GenerateSerializer]
public sealed record Robot
{
    [Id(0)]
    public Queue<string> Instructions { get; } = new();
}

public sealed class RobotGrain(
    ILogger<RobotGrain> logger,
    [PersistentState("Robot", "RobotStore")] IPersistentState<Robot> robot) : Grain, IRobotGrain
{
    public async Task AddInstruction(string instruction)
    {
        robot.State.Instructions.Enqueue(instruction);
        await robot.WriteStateAsync();

        logger.LogInformation($"{this.GetPrimaryKeyString()} <= {instruction}");
    }

    public async Task<string?> GetNextInstruction()
    {
        if (robot.State.Instructions.Count == 0) return null;

        var instruction = robot.State.Instructions.Dequeue();
        await robot.WriteStateAsync();

        logger.LogInformation($"{this.GetPrimaryKeyString()} => {instruction}");

        return instruction;
    }

    public Task<int> GetInstructionCount() =>
        Task.FromResult(robot.State.Instructions.Count);
}