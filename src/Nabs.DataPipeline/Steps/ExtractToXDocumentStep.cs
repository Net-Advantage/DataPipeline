namespace Nabs.DataPipeline.Steps;

public sealed class ExtractToXDocumentStep<TPipelineState>(TPipelineState pipelineState) 
	: Step<TPipelineState>(pipelineState)
	where TPipelineState : class, IPipelineState, IExtractXmlToXDocument
{
	public override async Task Transform()
	{
		var sourceFiles = await PipelineState.SourceConnection.Extract();

		foreach (var sourceFile in sourceFiles)
		{
			if (string.IsNullOrWhiteSpace(sourceFile.Content))
			{
				continue;
			}

			var xDocument = XDocument.Parse(sourceFile.Content!);
			PipelineState.SourceDocuments.Add((sourceFile.Path, xDocument));
		}
	}
}


public interface IExtractXmlToXDocument
{
	ISourceConnection<SourceFile[]> SourceConnection { get; }
	List<(string filePath, XDocument document)> SourceDocuments { get; }
}
