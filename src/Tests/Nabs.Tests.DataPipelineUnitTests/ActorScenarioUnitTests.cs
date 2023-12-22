using Nabs.DataPipeline;
using Nabs.DataPipeline.Steps;

namespace Nabs.Tests.DataPipelineUnitTests;

[UsesVerify]
public sealed class ActorScenarioUnitTests
{
	[Fact]
	public async Task TestFileBasedPipelineOrchestrator()
	{
		// Arrange
		var correlationId = new Guid("01219a58-d3cd-4653-ae84-4db74a6ac0d5");
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsOutput.json");
		
		var pipelineState = new TestActorsPipelineState()
		{
			CorrelationId = correlationId,
			SourceConnection = new FileSourceConnection(new FileSourceConnectionOptions(sourceFilePath)),
			DestinationConnection  = new FileDestinationConnection(new FileDestinationConnectionOptions(destinationFilePath))
		};
		pipelineState.PipelineOutput.CorrelationId = correlationId;

		var pipeline = new TestActorsPipeline(pipelineState);
		pipeline.AddStep<ExtractToXDocumentStep<TestActorsPipelineState>>();
		pipeline.AddStep<TestActorMappingStep>();

		// Act
		var result = await pipeline.Process();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = pipeline.PipelineState.PipelineOutput;
		pipelineOutput.CorrelationId.Should().Be(correlationId);

		await Verify(pipelineOutput);
	}

	[Fact]
	public async Task TestAzureBasedPipelineOrchestrator()
	{
		// Arrange
		var correlationId = new Guid("01219a58-d3cd-4653-ae84-4db74a6ac0d5");
		var sourceConnectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationConnectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "AzureTestActorsOutput.json");
		
		var pipelineState = new TestActorsPipelineState()
		{
			CorrelationId = correlationId,
			SourceConnection = new BlobSourceConnection(new BlobSourceConnectionOptions(sourceConnectionString)),
			DestinationConnection  = new BlobDestinationConnection(new BlobDestinationConnectionOptions(destinationConnectionString))
		};
		pipelineState.PipelineOutput.CorrelationId = correlationId;

		var pipeline = new TestActorsPipeline(pipelineState);
		pipeline.AddStep<ExtractToXDocumentStep<TestActorsPipelineState>>();
		pipeline.AddStep<TestActorMappingStep>();

		// Act
		var result = await pipeline.Process();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = pipeline.PipelineState.PipelineOutput;
		pipelineOutput.CorrelationId.Should().Be(correlationId);

		await Verify(pipelineOutput);
	}
}