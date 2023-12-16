namespace Nabs.DataPipeline.Steps;

/// <summary>
/// Takes a XDocument and loads it to a destination.
/// </summary>
/// <typeparam name="TStage"></typeparam>
/// <param name="stage"></param>
public sealed class XmlLoadingStep<TStage>
	: Step<TStage, ActivityEnd>
		where TStage : class, IStage, IXmlLoadingStepInput
{
	public XmlLoadingStep(TStage stage)
		: base(stage)
	{
		StepOutput = new ActivityEnd();
	}
	
	public override async Task Transform()
	{
		var content = "";
		await Stage.DestinationConnection.Load(content);
	}
}

public interface IXmlLoadingStepInput
{
	DestinationConnection<string> DestinationConnection { get; }
}