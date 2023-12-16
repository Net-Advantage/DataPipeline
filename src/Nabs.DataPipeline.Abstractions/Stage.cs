namespace Nabs.DataPipeline.Abstractions;

public interface IStage : IActivity;

public abstract class Stage<TPipeline, TStageOutput>(
	TPipeline pipeline) 
	: Activity, IStage
		where TPipeline : class, IPipeline
		where TStageOutput : class
{
	public LinkedList<IStep> Steps = new();

	public TPipeline Pipeline { get; } = pipeline;
	public TStageOutput? StageOutput { get; protected set; }

	protected override async Task<ActivityResult> ProcessActivity()
	{
		foreach (var step in Steps)
		{
			await step.Process();
		}
		
		await Transform();

		return ActivityError.None;
	}
}
