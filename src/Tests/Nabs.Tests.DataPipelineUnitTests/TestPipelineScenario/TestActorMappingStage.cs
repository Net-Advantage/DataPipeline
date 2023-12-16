using Nabs.DataPipeline.Abstractions;
using Nabs.DataPipeline.Connections;
using Nabs.DataPipeline.Steps;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario;

public sealed class TestActorMappingStage 
	: Stage<TestActorsPipeline, TestActorsPipelineOutput>, IXmlExtractionStepInput
{
	private readonly XmlExtractionStep<TestActorMappingStage> _xmlExtractionStep;

	public TestActorMappingStage(TestActorsPipeline pipeline) : base(pipeline)
	{
		StageOutput = Pipeline.PipelineOutput!;
		
		SourceConnection = new FileSourceConnection(Pipeline.PipelineInput.FileSourcePath);
		DestinationConnection = new FileDestinationConnection(Pipeline.PipelineInput.FileDestinationPath);

		_xmlExtractionStep = new XmlExtractionStep<TestActorMappingStage>(this);

		Steps.AddLast(_xmlExtractionStep);
	}

	public SourceConnection<string> SourceConnection { get; }
	public DestinationConnection<string> DestinationConnection { get; }

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
