using GrainInterfaces;

namespace Grains;

public sealed class Robot : Grain, IRobot
{
    private readonly Queue<string> _instructions = new();

    public Task AddInstruction(string instruction)
    {
        _instructions.Enqueue(instruction);
        return Task.CompletedTask;
    }

    public Task<string> GetNextInstruction()
    {
        return _instructions.Count == 0 ? Task.FromResult<string>(null) : Task.FromResult(_instructions.Dequeue());
    }

    public Task<int> GetInstructionCount()
    {
        return Task.FromResult(_instructions.Count);
    }
}