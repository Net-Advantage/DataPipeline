using Nabs.DataPipeline.Connections.Azure.StorageAccounts;

namespace Nabs.Tests.DataPipelineUnitTests;

[UsesVerify]
public sealed class PipelineOrchestratorUnitTest
{
	[Fact]
	public async Task TestFileBasedPipelineOrchestrator()
	{
		// Arrange
		var correlationId = new Guid("01219a58-d3cd-4653-ae84-4db74a6ac0d5");
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsOutput.json");
		
		var pipelineOptions = new TestActorsPipelineOutput(correlationId);
		var sourceConnection = new FileSourceConnection(new FileSourceConnectionOptions(sourceFilePath));
		var destinationConnection = new FileDestinationConnection(new FileDestinationConnectionOptions(destinationFilePath));

		var pipelineInput = new TestActorsPipelineOptions(
			correlationId, 
			pipelineOptions,
			sourceConnection,
			destinationConnection);

		var pipeline = new TestActorsPipeline(pipelineInput);
		PipelineOrchestrator<TestActorsPipeline> orchestrator = new(pipeline);

		// Act
		var result = await orchestrator.ExecuteAsync();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = orchestrator.Pipeline.PipelineOutput!;
		pipelineOutput.CorrelationId.Should().Be(pipelineInput.CorrelationId);

		await Verify(pipelineOutput);
	}

	[Fact]
	public async Task TestAzureBasedPipelineOrchestrator()
	{
		// Arrange
		var correlationId = new Guid("01219a58-d3cd-4653-ae84-4db74a6ac0d5");
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "AzureTestActorsOutput.json");
		
		var pipelineOptions = new TestActorsPipelineOutput(correlationId);
		var sourceConnection = new BlobSourceConnection(new BlobSourceConnectionOptions(sourceFilePath));
		var destinationConnection = new BlobDestinationConnection(new BlobDestinationConnectionOptions(destinationFilePath));

		var pipelineInput = new TestActorsPipelineOptions(
			correlationId, 
			pipelineOptions,
			sourceConnection,
			destinationConnection);

		var pipeline = new TestActorsPipeline(pipelineInput);
		PipelineOrchestrator<TestActorsPipeline> orchestrator = new(pipeline);

		// Act
		var result = await orchestrator.ExecuteAsync();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = orchestrator.Pipeline.PipelineOutput!;
		pipelineOutput.CorrelationId.Should().Be(pipelineInput.CorrelationId);

		await Verify(pipelineOutput);
	}
}