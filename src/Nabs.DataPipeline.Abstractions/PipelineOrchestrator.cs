namespace Nabs.DataPipeline.Abstractions;

public class PipelineOrchestrator<TPipeline>(TPipeline pipeline)
	where TPipeline : class, IPipeline
{
	public TPipeline Pipeline { get; } = pipeline;

	public async Task<ActivityResult> ExecuteAsync()
	{
		try
		{
			return await Pipeline.Process();
		}
		catch(Exception ex)
		{
			return ActivityResult.Failure(ex);
		}
	}
}
