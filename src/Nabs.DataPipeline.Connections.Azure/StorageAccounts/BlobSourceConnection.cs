namespace Nabs.DataPipeline.Connections.Azure.StorageAccounts;

public sealed class BlobSourceConnection(BlobSourceConnectionOptions connectionOptions)
		: SourceConnection<BlobSourceConnectionOptions, string>(connectionOptions)
{
	public override async Task<string?> Extract()
	{
		//TODO: DWS: Implement Azure Blob
		var sourceFile = await File.ReadAllTextAsync(ConnectionOptions.ConnectionString);
		return sourceFile;
	}
}

public sealed record BlobSourceConnectionOptions(string ConnectionString)
	: IConnectionOptions;