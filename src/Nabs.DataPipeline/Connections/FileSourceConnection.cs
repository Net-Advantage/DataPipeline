namespace Nabs.DataPipeline.Connections;


public sealed class FileSourceConnection(FileSourceConnectionOptions connectionOptions)
		: SourceConnection<FileSourceConnectionOptions, SourceFile[]>(connectionOptions)
{
	public override async Task<SourceFile[]> Extract()
	{
		if (ConnectionOptions.SourcePath.Contains('*'))
		{
			var batchResult = await ProcessWildcardPath();
			return batchResult;
		}

		var sourceFile = await File.ReadAllTextAsync(ConnectionOptions.SourcePath);
		return [ 
			new SourceFile() 
			{
				Path = ConnectionOptions.SourcePath, 
				Content = sourceFile 
			}];
	}

	private async Task<SourceFile[]> ProcessWildcardPath()
	{
		var directoryPath = Path.GetDirectoryName(ConnectionOptions.SourcePath!)!;
		var searchPattern = Path.GetFileName(ConnectionOptions.SourcePath);

		string[] matchingFiles = Directory.GetFiles(directoryPath, searchPattern);

		var fileContents = new List<SourceFile>();
		foreach (string filePath in matchingFiles)
		{
			var fileContent = await File.ReadAllTextAsync(filePath);
			fileContents.Add(new SourceFile()
			{
				Path = filePath,
				Content = fileContent
			});
		}

		return [.. fileContents];
	}
}

public sealed record FileSourceConnectionOptions(string SourcePath)
	: IConnectionOptions;
