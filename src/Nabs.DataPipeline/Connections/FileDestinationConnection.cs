namespace Nabs.DataPipeline.Connections;

public sealed class FileDestinationConnection(string DestinationPath)
	: DestinationConnection<string>
{
	public override Task Load(string content)
	{
		return File.WriteAllTextAsync(DestinationPath, content);
	}
}

