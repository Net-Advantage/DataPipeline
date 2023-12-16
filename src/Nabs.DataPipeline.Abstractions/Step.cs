namespace Nabs.DataPipeline.Abstractions;

public interface IStep : IActivity;

public abstract class Step<TStage, TStepOutput>(
	TStage stage)
	: Activity, IStep
		where TStage : class, IStage
		where TStepOutput : class
{
	public TStage Stage { get; } = stage;
	public TStepOutput? StepOutput { get; protected set; }

	protected override async Task<ActivityResult> ProcessActivity()
	{
		await Transform();

		return ActivityError.None;
	}
}
