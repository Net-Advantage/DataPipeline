namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed record TestActorsPipelineOutput(
    Guid CorrelationId)
{
    public List<TestActor> TestActors { get; } = [];
}

public record TestActor
{
    public string SourceFile { get; set; } = default!;
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}


public sealed record TestActorsPipelineOptions(
    Guid CorrelationId,
    TestActorsPipelineOutput PipelineOutput,
    ISourceConnection<SourceFile[]> SourceConnection,
    IDestinationConnection<string> DestinationConnection);