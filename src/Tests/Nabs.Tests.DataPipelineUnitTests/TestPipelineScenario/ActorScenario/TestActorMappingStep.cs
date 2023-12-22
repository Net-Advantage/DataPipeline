namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public sealed class TestActorMappingStep(TestActorsPipelineState pipelineState) 
	: Step<TestActorsPipelineState>(pipelineState)
{
	public override Task Transform()
	{
		var people = PipelineState.SourceDocuments[0].document
			.Descendants("Person")
			.Select(ParsePersonElement);	

		PipelineState.PipelineOutput.TestActors.AddRange(people);

		return Task.CompletedTask;
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