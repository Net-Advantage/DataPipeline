namespace Nabs.DataPipeline.Connections;

public sealed class FileDestinationConnection(FileDestinationConnectionOptions connectionOptions)
		: DestinationConnection<FileDestinationConnectionOptions, string>(connectionOptions)
{
	public override Task Load(string content)
	{
		return File.WriteAllTextAsync(ConnectionOptions.DestinationPath, content);
	}
}

public sealed record FileDestinationConnectionOptions(string DestinationPath)
	: IConnectionOptions;