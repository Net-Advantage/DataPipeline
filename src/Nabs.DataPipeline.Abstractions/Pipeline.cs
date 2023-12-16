namespace Nabs.DataPipeline.Abstractions;

public interface IPipeline : IActivity;

public abstract class Pipeline<TPipelineInput, TPipelineOutput>(
	TPipelineInput pipelineInput) 
	: Activity, IPipeline
		where TPipelineInput : class
		where TPipelineOutput : class
{
	public List<IStage> Stages { get; } = [];
	public TPipelineInput PipelineInput { get; } = pipelineInput;
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
