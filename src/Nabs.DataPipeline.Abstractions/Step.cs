namespace Nabs.DataPipeline;

public interface IStep<TPipelineState>
	where TPipelineState : class, IPipelineState
{
	TPipelineState PipelineState { get; }

	Task Transform();
}

public abstract class Step<TPipelineState> : IStep<TPipelineState>
	where TPipelineState : class, IPipelineState
{
	protected Step(TPipelineState pipelineState)
	{
		PipelineState = pipelineState;
	}

	public TPipelineState PipelineState { get; }

	public abstract Task Transform();
}