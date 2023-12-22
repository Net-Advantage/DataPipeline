namespace Nabs.DataPipeline;

public enum ActivityStatus
{
	NotStarted,
	InProgress,
	Completed
}

public sealed class ActivityEnd
{

}

public sealed class ActivityResult
{
	private ActivityResult(bool isSuccess, ActivityError error, Exception? exception = null)
	{

		if ((isSuccess && (error != ActivityError.None || exception != null)) ||
		(!isSuccess && (error == ActivityError.None && exception == null)) ||
		(!isSuccess && (error != ActivityError.None && exception != null)))
	
		{
			throw new ArgumentException("Invalid combination of success, error, and exception states.", nameof(isSuccess));
		}

		IsSuccess = isSuccess;
		Error = error;
		Exception = exception;
	}

	public bool IsSuccess { get; }
	public ActivityError Error { get; }
	public Exception? Exception { get; }

	public static ActivityResult Success() => new(true, ActivityError.None);
	public static ActivityResult Failure(ActivityError error) => new(false, error);
	public static ActivityResult Failure(Exception exception) => new(false, ActivityError.None, exception);
}

public sealed record ActivityError(string Code, string Description)
{
	public static readonly ActivityError None = new(string.Empty, string.Empty);

	public static implicit operator ActivityResult(ActivityError error) => error == None 
		? ActivityResult.Success() 
		: ActivityResult.Failure(error);
}