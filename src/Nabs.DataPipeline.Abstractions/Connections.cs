namespace Nabs.DataPipeline;

public interface IConnection
{
}


public interface ISourceConnection<T>
{
	abstract Task<T> Extract();
}

public interface IDestinationConnection<T>
{
	abstract Task Load(T content);
}

public abstract class SourceConnection<TConnectionOptions, T> : ISourceConnection<T>
	where TConnectionOptions : IConnectionOptions
{
	protected SourceConnection(TConnectionOptions connectionOptions)
	{
		ConnectionOptions = connectionOptions;
	}

	protected TConnectionOptions ConnectionOptions { get; }

	public abstract Task<T> Extract();
}

public abstract class DestinationConnection<TConnectionOptions, T> : IDestinationConnection<T>
	where TConnectionOptions : class, IConnectionOptions
{
	protected DestinationConnection(TConnectionOptions connectionOptions)
	{
		ConnectionOptions = connectionOptions;
	}

	protected TConnectionOptions ConnectionOptions { get; }

	public abstract Task Load(T content);
}


public interface IConnectionOptions;