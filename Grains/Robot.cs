using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using GrainInterfaces;

namespace Grains;

[GenerateSerializer]
public class RobotState
{
    [Id(0)]
    public Queue<string> Instructions { get; } = new();
}

public sealed class Robot(
    ILogger<Robot> logger,
    [PersistentState("robotState", "robotStateStore")] IPersistentState<RobotState> state) : Grain, IRobot
{
    public async Task AddInstruction(string instruction)
    {
        state.State.Instructions.Enqueue(instruction);
        await state.WriteStateAsync();

        logger.LogInformation($"{this.GetPrimaryKeyString()} <= {instruction}");
    }

    public async Task<string?> GetNextInstruction()
    {
        if (state.State.Instructions.Count == 0) return null;

        var instruction = state.State.Instructions.Dequeue();
        await state.WriteStateAsync();

        logger.LogInformation($"{this.GetPrimaryKeyString()} => {instruction}");

        return instruction;
    }

    public Task<int> GetInstructionCount() =>
        Task.FromResult(state.State.Instructions.Count);
}