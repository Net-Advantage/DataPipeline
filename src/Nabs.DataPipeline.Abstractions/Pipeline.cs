namespace Nabs.DataPipeline.Abstractions;

public interface IPipeline : IActivity;

public abstract class Pipeline<TPipelineOptions, TPipelineOutput>(
	TPipelineOptions pipelineOptions) 
	: Activity, IPipeline
		where TPipelineOptions : class
		where TPipelineOutput : class
{
	public List<IStage> Stages { get; } = [];
	public TPipelineOptions PipelineOptions { get; } = pipelineOptions;
	public TPipelineOutput? PipelineOutput { get; protected set; }
	
	protected override async Task<ActivityResult> ProcessActivity()
	{
		foreach (var stage in Stages)
		{
			await stage.Process();
		}
		
		await Transform();
		
		return ActivityError.None;
	}
}
