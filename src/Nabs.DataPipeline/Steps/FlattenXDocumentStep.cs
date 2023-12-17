
namespace Nabs.DataPipeline.Steps;

public sealed class FlattenXDocumentStep<TStage, TModel>(TStage stage, Func<TModel> flattenTransformation)
	: Step<TStage, TModel>(stage)
	where TStage : class, IStage, IXDocumentExtractionStepInput<TModel>
	where TModel : class, new()
{
	private readonly Func<TModel> _flattenTransformation = flattenTransformation;

	public override Task Transform()
	{
		StepOutput = _flattenTransformation();

		return Task.CompletedTask;
	}
}

public interface IXDocumentExtractionStepInput<TModel> 
	where TModel : class, new()
{

}