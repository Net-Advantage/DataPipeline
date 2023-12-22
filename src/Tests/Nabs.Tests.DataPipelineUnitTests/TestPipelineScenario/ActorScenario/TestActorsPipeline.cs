namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed class TestActorsPipeline(TestActorsPipelineState pipelineState)
	: Pipeline<TestActorsPipelineState>(pipelineState)
{
}