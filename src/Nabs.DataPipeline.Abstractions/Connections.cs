namespace Nabs.DataPipeline.Abstractions;

public interface IConnection
{
}

public abstract class Connection : IConnection
{

}

public abstract class SourceConnection<T> : Connection
{
	public abstract Task<T?> Extract();
}

public abstract class DestinationConnection<T> : Connection
{
	public abstract Task Load(T content);
}
