namespace Nabs.DataPipeline;

public sealed record SourceFile
{
	public string Path { get; set; } = default!;
	public string? Content { get; set; }
}