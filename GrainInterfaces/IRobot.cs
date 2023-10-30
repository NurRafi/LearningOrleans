namespace GrainInterfaces;

public interface IRobot : IGrainWithStringKey
{
    Task AddInstruction(string instruction);
    Task<string?> GetNextInstruction();
    Task<int> GetInstructionCount();
}