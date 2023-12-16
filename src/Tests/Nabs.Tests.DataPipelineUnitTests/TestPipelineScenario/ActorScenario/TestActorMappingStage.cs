namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed class TestActorMappingStage
	: Stage<TestActorsPipeline, TestActorsPipelineOutput>, 
		IXmlExtractionStepInput<string>
{
	private readonly XmlToStringExtractionStep<TestActorMappingStage> _xmlExtractionStep;

	public TestActorMappingStage(TestActorsPipeline pipeline) : base(pipeline)
	{
		StageOutput = Pipeline.PipelineOutput!;
		SourceConnection = Pipeline.PipelineOptions.SourceConnection;
		DestinationConnection = Pipeline.PipelineOptions.DestinationConnection;

		_xmlExtractionStep = new XmlToStringExtractionStep<TestActorMappingStage>(this);

		Steps.AddLast(_xmlExtractionStep);
	}

	public ISourceConnection<string> SourceConnection { get; }
	public IDestinationConnection<string> DestinationConnection { get; }
	

	public override async Task Transform()
	{
		if (_xmlExtractionStep.StepOutput is null)
		{
			await Task.CompletedTask;
			return;
		}

		var people = _xmlExtractionStep.StepOutput!
			.Descendants("Person")
			.Select(ParsePersonElement);

		Pipeline.PipelineOutput!.TestActors.AddRange(people);

		var json = JsonConvert.SerializeObject(Pipeline.PipelineOutput, Formatting.Indented);
		await DestinationConnection.Load(json);

		await Task.CompletedTask;
	}

	private TestActor ParsePersonElement(XElement element)
	{
		var result = new TestActor()
		{
			Id = element.Attribute("id").ExtractGuidValue(),
			FirstName = element.Element("FirstName").ExtractStringValue(),
			LastName = element.Element("LastName").ExtractStringValue(),
			DateOfBirth = element.Element("DateOfBirth").ExtractDateOnlyValue()
		};

		return result;
	}
}
