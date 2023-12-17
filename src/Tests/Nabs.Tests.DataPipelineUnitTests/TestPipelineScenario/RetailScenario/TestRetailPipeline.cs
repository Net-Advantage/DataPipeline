namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.RetailScenario;

public sealed class TestRetailPipeline : Pipeline<TestRetailPipelineOptions, TestRetailPipelineOutput>
{
	private readonly TestRetailStage _testRetailStage;

	public TestRetailPipeline(TestRetailPipelineOptions pipelineOptions) 
		: base(pipelineOptions)
	{
		PipelineOutput = pipelineOptions.PipelineOutput;

		_testRetailStage = new(this);

		Stages.Add(_testRetailStage);
	}

	public override Task Transform()
	{
		if (PipelineOutput is null)
		{
			return Task.CompletedTask;
		}

		PipelineOutput = _testRetailStage.StageOutput!;
		return Task.CompletedTask;
	}
}
