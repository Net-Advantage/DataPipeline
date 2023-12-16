namespace Nabs.Tests.DataPipelineUnitTests.TestPipelineScenario.ActorScenario;

public record TestActor
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}