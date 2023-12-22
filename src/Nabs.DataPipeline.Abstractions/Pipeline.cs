namespace Nabs.DataPipeline;

public interface IPipelineState
{

}

public interface IPipeline<TPipelineState>
	where TPipelineState : class, IPipelineState
{
	string ActivityName { get; }
	ActivityStatus ActivityStatus { get; }

	Task<ActivityResult> Process();

	void AddStep<TStep>()
		where TStep : class, IStep<TPipelineState>;
}

public abstract class Pipeline<TPipelineState>
	: IPipeline<TPipelineState>
	where TPipelineState : class, IPipelineState
{
	private readonly List<IStep<TPipelineState>> _steps = [];

	protected Pipeline(TPipelineState pipelineState)
	{
		ActivityName = GetType().Name;

		PipelineState = pipelineState;
	}

	public TPipelineState PipelineState { get; set; }

	public void AddStep<TStep>()
		where TStep : class, IStep<TPipelineState>
	{
		var step = (TStep)Activator.CreateInstance(typeof(TStep), new[] { PipelineState })!;
		_steps.Add(step);
	}

	public string ActivityName { get; protected set; }
	public ActivityStatus ActivityStatus { get; private set; } = ActivityStatus.NotStarted;

	public async Task<ActivityResult> Process()
	{
		ActivityStatus = ActivityStatus.InProgress;
		try
		{
			foreach (var step in _steps)
			{
				await step.Transform();
			}
			return ActivityResult.Success();
		}
		catch (Exception ex)
		{
			return ActivityResult.Failure(ex);
		}
		finally
		{
			ActivityStatus = ActivityStatus.Completed;
		}
	}
}
