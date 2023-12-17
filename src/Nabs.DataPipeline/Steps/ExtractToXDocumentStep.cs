namespace Nabs.DataPipeline.Steps;

/// <summary>
/// Parse an XML string and outputs an XDocument.
/// </summary>
/// <typeparam name="TStage"></typeparam>
/// <param name="stage"></param>
public sealed class ExtractToXDocumentStep<TStage>(TStage stage)
	: Step<TStage, (string filePath, XDocument document)[]>(stage)
		where TStage : class, IStage, IExtractToXDocumentStepOptions

{
	public override async Task Transform()
	{
		var sourceFiles = await Stage.SourceConnection.Extract();

		var output = new List<(string filePath, XDocument document)>();
		foreach (var sourceFile in sourceFiles)
		{
			if (string.IsNullOrWhiteSpace(sourceFile.Content))
			{
				continue;
			}

			var xDocument = XDocument.Parse(sourceFile.Content!);
			output.Add((sourceFile.Path, xDocument));

		}

		StepOutput = [.. output];
	}
}


public interface IExtractToXDocumentStepOptions
{
	ISourceConnection<SourceFile[]> SourceConnection { get; }
}
