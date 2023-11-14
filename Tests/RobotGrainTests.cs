using GrainInterfaces;

namespace Tests;

[Collection(ClusterCollection.Name)]
public class RobotGrainTests(ClusterFixture fixture)
{
    [Fact(DisplayName = "Instructions work")]
    public async Task InstructionsWork()
    {
        var robotGrain = fixture.Cluster.GrainFactory.GetGrain<IRobotGrain>("Nur");

        await robotGrain.AddInstruction("Do the dishes");
        await robotGrain.AddInstruction("Take out the trash");
        await robotGrain.AddInstruction("Do the laundry");

        Assert.Equal(3, await robotGrain.GetInstructionCount());

        Assert.Equal("Do the dishes", await robotGrain.GetNextInstruction());
        Assert.Equal("Take out the trash", await robotGrain.GetNextInstruction());
        Assert.Equal("Do the laundry", await robotGrain.GetNextInstruction());

        Assert.Null(await robotGrain.GetNextInstruction());

        Assert.Equal(0, await robotGrain.GetInstructionCount());
    }
}