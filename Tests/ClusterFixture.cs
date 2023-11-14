using Orleans.TestingHost;

namespace Tests;

public class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; }

    public ClusterFixture()
    {
        Cluster = new TestClusterBuilder().AddSiloBuilderConfigurator<TestSiloConfig>().Build();
        Cluster.Deploy();
    }

    public void Dispose() => Cluster.StopAllSilos();
}

public class TestSiloConfig : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("RobotStore");
    }
}