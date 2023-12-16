namespace Nabs.DataPipeline.Connections.Azure.StorageAccounts;

public sealed class BlobSourceConnection(BlobSourceConnectionOptions connectionOptions)
		: SourceConnection<BlobSourceConnectionOptions, string>(connectionOptions)
{
	public override async Task<string?> Extract()
	{
		//TODO: DWS: Implement'
		string? result = "";
		return await Task.FromResult(result);
	}
}

public sealed record BlobSourceConnectionOptions(string ConnectionString)
	: IConnectionOptions;