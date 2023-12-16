namespace Nabs.DataPipeline.Connections;

public sealed class FileSourceConnection(string SourcePath)
	: SourceConnection<string>
{
	public override async Task<string?> Extract()
	{
		var sourceFile = await File.ReadAllTextAsync(SourcePath);
		return sourceFile;
	}
};

