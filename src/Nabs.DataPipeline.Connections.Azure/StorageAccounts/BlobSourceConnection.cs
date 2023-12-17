namespace Nabs.DataPipeline.Connections.Azure.StorageAccounts;

public sealed class BlobSourceConnection(BlobSourceConnectionOptions connectionOptions)
		: SourceConnection<BlobSourceConnectionOptions, SourceFile[]>(connectionOptions)
{
	public override async Task<SourceFile[]> Extract()
	{
		//TODO: DWS: Implement Azure Blob
		var sourceFile = await File.ReadAllTextAsync(ConnectionOptions.ConnectionString);
		return [ new SourceFile() 
		{ 
			Path = ConnectionOptions.ConnectionString, 
			Content = sourceFile 
		}];
	}
}

public sealed record BlobSourceConnectionOptions(string ConnectionString)
	: IConnectionOptions;