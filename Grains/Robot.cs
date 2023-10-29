using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Grains;

public sealed class Robot(ILogger<Robot> logger) : Grain, IRobot
{
    private readonly Queue<string> _instructions = new();

    public Task AddInstruction(string instruction)
    {
        _instructions.Enqueue(instruction);

        logger.LogInformation($"{this.GetPrimaryKeyString()} <= {instruction}");

        return Task.CompletedTask;
    }

    public Task<string> GetNextInstruction()
    {
        var instruction = _instructions.Dequeue();

        logger.LogInformation($"{this.GetPrimaryKeyString()} => {instruction}");

        // Wish there was Option
        return _instructions.Count == 0 ? Task.FromResult<string>(null) : Task.FromResult(instruction);
    }

    public Task<int> GetInstructionCount() =>
        Task.FromResult(_instructions.Count);
}