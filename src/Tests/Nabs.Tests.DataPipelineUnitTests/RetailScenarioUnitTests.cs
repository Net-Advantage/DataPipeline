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
		
		var pipelineState = new TestRetailPipelineState(
			correlationId, 
			new TestRetailPipelineOutput(correlationId), 
			new FileSourceConnection(new FileSourceConnectionOptions(sourceFilePath)),
			new FileDestinationConnection(new FileDestinationConnectionOptions(destinationFilePath)));


		var pipeline = new TestRetailPipeline(pipelineState);
		pipeline.AddStep<ExtractToXDocumentStep<TestRetailPipelineState>>();
		pipeline.AddStep<TestRetailStep>();

		// Act
		var result = await pipeline.Process();

		// Arrange
		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = pipeline.PipelineState.PipelineOutput;
		pipelineOutput.CorrelationId.Should().Be(correlationId);

		await Verify(pipelineOutput);
	}
}
