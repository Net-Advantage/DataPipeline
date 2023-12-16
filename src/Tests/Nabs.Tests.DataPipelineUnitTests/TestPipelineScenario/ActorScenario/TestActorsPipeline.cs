using Nabs.DataPipeline.Connections.Azure.StorageAccounts;

namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed class TestActorsPipeline
    : Pipeline<TestActorsPipelineOptions, TestActorsPipelineOutput>
{
    private readonly TestActorMappingStage _testActorMappingStage;

    public TestActorsPipeline(TestActorsPipelineOptions pipelineOptions)
        : base(pipelineOptions)
    {
        PipelineOutput = pipelineOptions.PipelineOutput;

        _testActorMappingStage = new TestActorMappingStage(this);

        Stages.Add(_testActorMappingStage);
    }

    public override Task Transform()
    {
        if (PipelineOutput is null)
        {
            return Task.CompletedTask;
        }

        PipelineOutput = _testActorMappingStage.StageOutput!;
        return Task.CompletedTask;
    }
}


public record TestActorsPipelineOptions(
    Guid CorrelationId,
    TestActorsPipelineOutput PipelineOutput,
    ISourceConnection<string> SourceConnection,
    IDestinationConnection<string> DestinationConnection);