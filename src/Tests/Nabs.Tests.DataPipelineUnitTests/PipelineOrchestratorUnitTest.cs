using FluentAssertions;
using Nabs.DataPipeline.Abstractions;
using Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario;

namespace Nabs.Tests.DataPipelineUnitTests;

[UsesVerify]
public sealed class PipelineOrchestratorUnitTest
{
	private readonly PipelineOrchestrator<TestActorsPipeline> _orchestrator;
	private readonly TestActorsPipelineInput _pipelineInput;

	public PipelineOrchestratorUnitTest()
	{
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsOutput.json");
		
		_pipelineInput = new TestActorsPipelineInput(
			Guid.NewGuid(), 
			sourceFilePath,
			destinationFilePath);

		var pipeline = new TestActorsPipeline(_pipelineInput);
		_orchestrator = new(pipeline);
	}

	[Fact]
	public async Task TestPipelineOrchestrator()
	{
		var result = await _orchestrator.ExecuteAsync();

		result.IsSuccess.Should().BeTrue();

		var pipelineOutput = _orchestrator.Pipeline.PipelineOutput!;
		pipelineOutput.CorrelationId.Should().Be(_pipelineInput.CorrelationId);

		await Verify(pipelineOutput);
	}
}