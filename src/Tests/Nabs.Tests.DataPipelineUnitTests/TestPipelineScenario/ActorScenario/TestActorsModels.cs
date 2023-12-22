
namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed class TestActorsPipelineState : IPipelineState, IExtractXmlToXDocument
{
	public TestActorsPipelineOutput PipelineOutput { get; set; } = new();

	public Guid CorrelationId { get; set; }

	public ISourceConnection<SourceFile[]> SourceConnection { get; set; } = default!;
	public IDestinationConnection<string> DestinationConnection { get; set; } = default!;
	public List<(string filePath, XDocument document)> SourceDocuments { get; } = [];
}


public sealed class TestActorsPipelineOutput
{
	public Guid CorrelationId { get; set; }
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