namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.RetailScenario;

public sealed class TestRetailPipeline(TestRetailPipelineState pipelineState)
	: Pipeline<TestRetailPipelineState>(pipelineState)
{
}
