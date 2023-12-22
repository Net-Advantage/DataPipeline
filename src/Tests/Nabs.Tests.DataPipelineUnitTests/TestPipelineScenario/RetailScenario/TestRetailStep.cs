namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.RetailScenario;

public sealed class TestRetailStep(TestRetailPipelineState pipelineState) 
	: Step<TestRetailPipelineState>(pipelineState)
{
	public override async Task Transform()
	{
		var flatten = Flatten();

		var retailers = flatten
			.Select(retailer => new Retailer()
			{
				Id = retailer.RetailerId,
				Name = retailer.RetailerName
			})
			.Distinct()
			.ToList();

		var customers = flatten
			.Select(customer => new Customer()
			{
				Id = customer.CustomerId,
				Name = customer.CustomerName,
				Address = customer.CustomerAddress
			})
			.Distinct()
			.ToList();

		var consumers = flatten
			.Select(consumer => new Consumer()
			{
				Id = consumer.ConsumerId,
				FirstName = consumer.ConsumerFirstName,
				LastName = consumer.ConsumerLastName,
				PrimaryShopper = consumer.ConsumerPrimaryShopper
			})
			.Distinct()
			.ToList();

		PipelineState.PipelineOutput.Retailers.AddRange(retailers);
		PipelineState.PipelineOutput.Customers.AddRange(customers);
		PipelineState.PipelineOutput.Consumers.AddRange(consumers);

		var json = JsonConvert.SerializeObject(PipelineState.PipelineOutput, Formatting.Indented);
		await PipelineState.DestinationConnection.Load(json);
	}

	private List<TestRetailFlattenModel> Flatten()
	{
		var result = new List<TestRetailFlattenModel>();
		foreach (var (filePath, document) in PipelineState.SourceDocuments)
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