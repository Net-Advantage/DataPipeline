namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.RetailScenario;


public sealed record TestRetailPipelineOptions(
	Guid CorrelationId,
	TestRetailPipelineOutput PipelineOutput,
	ISourceConnection<SourceFile[]> SourceConnection,
	IDestinationConnection<string> DestinationConnection);

public sealed record TestRetailPipelineOutput(
	Guid CorrelationId)
{
	public List<Retailer> Retailers { get; set; } = [];
	public List<Customer> Customers { get; set; } = [];
	public List<Consumer> Consumers { get; set; } = [];
};

public sealed record TestRetailFlattenModel
{
	public string SourceFile { get; set; } = default!;
	public Guid RetailerId { get; set; }
	public string RetailerName { get; set; } = default!;
	public Guid CustomerId { get; set; }
	public string CustomerName { get; set; } = default!;
	public string CustomerAddress { get; set; } = default!;
	public Guid ConsumerId { get; set; }
	public string ConsumerFirstName { get; set; } = default!;
	public string ConsumerLastName { get; set; } = default!;
	public bool ConsumerPrimaryShopper { get; set; }
};

public sealed record Retailer
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
}

public sealed record Customer
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string Address { get; set; } = default!;
}

public sealed record Consumer
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public bool PrimaryShopper { get; set; }
}