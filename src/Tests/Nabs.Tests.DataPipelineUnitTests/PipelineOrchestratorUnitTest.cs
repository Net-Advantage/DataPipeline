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
		var correlationId = new Guid("01219a58-d3cd-4653-ae84-4db74a6ac0d5");
		var sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsInput.xml");
		var destinationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "SourceFiles", "TestActorsOutput.json");
		
		_pipelineInput = new TestActorsPipelineInput(
			correlationId, 
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