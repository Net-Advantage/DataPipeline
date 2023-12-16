namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public record TestActorsPipelineOutput(
    Guid CorrelationId)
{
    public List<TestActor> TestActors { get; } = [];
}
