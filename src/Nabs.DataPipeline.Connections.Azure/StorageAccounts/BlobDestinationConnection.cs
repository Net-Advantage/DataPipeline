namespace Nabs.DataPipeline.Connections.Azure.StorageAccounts;


public sealed class BlobDestinationConnection(BlobDestinationConnectionOptions connectionOptions)
		: DestinationConnection<BlobDestinationConnectionOptions, string>(connectionOptions)
{
	public override Task Load(string content)
	{
		//TODO: DWS: Implement
		return Task.CompletedTask;
	}
}

public sealed record BlobDestinationConnectionOptions(string ConnectionString)
	: IConnectionOptions;