namespace Nabs.DataPipeline.Connections;


public sealed class FileSourceConnection(FileSourceConnectionOptions connectionOptions)
		: SourceConnection<FileSourceConnectionOptions, string>(connectionOptions)
{
	public override async Task<string?> Extract()
	{
		var sourceFile = await File.ReadAllTextAsync(ConnectionOptions.SourcePath);
		return sourceFile;
	}
}

public sealed record FileSourceConnectionOptions(string SourcePath)
	: IConnectionOptions;