namespace Nabs.Tests.DataPipelineUnitTests;

[UsesVerify]
public sealed class RetailScenarioUnitTests
{
	[Fact]
	public async Task TestFileBasedPipelineOrchestrator()
	{
		// Arrange
		var correlationId = new Guid("efc4d7c9-7fc0-454a-86cc-0d7cd29bcbfb");
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestRetailInput*.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestRetailOutput.json");
		
		var pipelineOptions = new TestRetailPipelineOutput(correlationId);
		var sourceConnection = new FileSourceConnection(new FileSourceConnectionOptions(sourceFilePath));
		var destinationConnection = new FileDestinationConnection(new FileDestinationConnectionOptions(destinationFilePath));

		var pipelineInput = new TestRetailPipelineOptions(
			correlationId, 
			pipelineOptions,
			sourceConnection,
			destinationConnection);

		var pipeline = new TestRetailPipeline(pipelineInput);
		PipelineOrchestrator<TestRetailPipeline> orchestrator = new(pipeline);

		// Act
		var result = await orchestrator.ExecuteAsync();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = orchestrator.Pipeline.PipelineOutput!;
		pipelineOutput.CorrelationId.Should().Be(pipelineInput.CorrelationId);

		await Verify(pipelineOutput);
	}
}
