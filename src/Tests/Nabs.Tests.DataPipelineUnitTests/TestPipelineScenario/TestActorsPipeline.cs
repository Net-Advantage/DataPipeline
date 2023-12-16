using Nabs.DataPipeline.Abstractions;

namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario;

public sealed class TestActorsPipeline : Pipeline<TestActorsPipelineInput, TestActorsPipelineOutput>
{
	private readonly TestActorMappingStage _testActorMappingStage;

	public TestActorsPipeline(TestActorsPipelineInput pipelineInput)
		: base(pipelineInput)
	{
		PipelineOutput = new TestActorsPipelineOutput(pipelineInput.CorrelationId);

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

public record TestActorsPipelineInput(
	Guid CorrelationId,
	string FileSourcePath,
	string FileDestinationPath);

public record TestActorsPipelineOutput(
	Guid CorrelationId)
{
	public List<TestActor> TestActors { get; } = [];
}

public record TestActor
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateOnly DateOfBirth { get; set; }
}