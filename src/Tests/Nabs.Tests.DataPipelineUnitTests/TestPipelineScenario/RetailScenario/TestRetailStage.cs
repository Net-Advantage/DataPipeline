
using Nabs.DataPipeline;

namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.RetailScenario;

public sealed class TestRetailStage
	: Stage<TestRetailPipeline, TestRetailPipelineOutput>,
		IExtractToXDocumentStepOptions,
		IXDocumentExtractionStepInput<List<TestRetailFlattenModel>>
{
	private readonly ExtractToXDocumentStep<TestRetailStage> _xmlExtractionStep;
	private readonly FlattenXDocumentStep<TestRetailStage, List<TestRetailFlattenModel>> _xDocumentFlattenStep;

	public TestRetailStage(TestRetailPipeline pipeline)
		: base(pipeline)
	{
		StageOutput = Pipeline.PipelineOutput!;

		_xmlExtractionStep = new(this);
		_xDocumentFlattenStep = new(this, FlattenTestRetailFunc);

		Steps.AddLast(_xmlExtractionStep);
		Steps.AddLast(_xDocumentFlattenStep);
	}

	public ISourceConnection<SourceFile[]> SourceConnection => Pipeline.PipelineOptions.SourceConnection;
	public IDestinationConnection<string> DestinationConnection => Pipeline.PipelineOptions.DestinationConnection;

	public override async Task Transform()
	{
		var retailers = _xDocumentFlattenStep.StepOutput!
			.Select(retailer => new Retailer()
			{
				Id = retailer.RetailerId,
				Name = retailer.RetailerName
			})
			.Distinct()
			.ToList();

		var customers = _xDocumentFlattenStep.StepOutput!
			.Select(customer => new Customer()
			{
				Id = customer.CustomerId,
				Name = customer.CustomerName,
				Address = customer.CustomerAddress
			})
			.Distinct()
			.ToList();

		var consumers = _xDocumentFlattenStep.StepOutput!
			.Select(consumer => new Consumer()
			{
				Id = consumer.ConsumerId,
				FirstName = consumer.ConsumerFirstName,
				LastName = consumer.ConsumerLastName,
				PrimaryShopper = consumer.ConsumerPrimaryShopper
			})
			.Distinct()
			.ToList();

		StageOutput!.Retailers.AddRange(retailers);
		StageOutput!.Customers.AddRange(customers);
		StageOutput!.Consumers.AddRange(consumers);

		var json = JsonConvert.SerializeObject(StageOutput, Formatting.Indented);
		await DestinationConnection.Load(json);
	}

	private List<TestRetailFlattenModel> FlattenTestRetailFunc()
	{
		var result = new List<TestRetailFlattenModel>();
		foreach (var (filePath, document) in _xmlExtractionStep.StepOutput!)
		{
			var flattenedData = document
			   .Descendants("Retailer")
			   .SelectMany(retailer => retailer.Descendants("Customer")
				   .SelectMany(customer => customer.Descendants("Consumer")
					   .Select(consumer => new TestRetailFlattenModel
					   {
						   SourceFile = filePath,
						   RetailerId = retailer.Attribute("id").ExtractGuidValue(),
						   RetailerName = retailer.Element("Name").ExtractStringValue(),
						   CustomerId = customer.Attribute("id").ExtractGuidValue(),
						   CustomerName = customer.Element("Name").ExtractStringValue(),
						   CustomerAddress = customer.Element("Address").ExtractStringValue(),
						   ConsumerId = consumer.Attribute("id").ExtractGuidValue(),
						   ConsumerFirstName = consumer.Element("GivenName").ExtractStringValue(),
						   ConsumerLastName = consumer.Element("FamilyName").ExtractStringValue(),
						   ConsumerPrimaryShopper = consumer.Element("PrimaryShopper").ExtractBooleanValue()
					   })
				   )
			   ).ToList();

			result.AddRange(flattenedData);
		}

		return result;
	}
}
