namespace TaskTracker.Application.Common.Models;

public class Result<T>
{
	internal Result(bool succeeded, T resultData, IEnumerable<string> errors)
	{
		Succeeded = succeeded;
		ResultData = resultData;
		Errors = errors.ToArray();
	}

	public T ResultData { get; set; }

	public bool Succeeded { get; set; }

	public string[] Errors { get; set; }

	public static Result<T> Success(T resultData)
	{
		return new Result<T>(true, resultData, Array.Empty<string>());
	}

	public static Result<T?> Failure(IEnumerable<string> errors)
	{
		return new Result<T?>(false, default, errors);
	}

	public static Result<T?> Failure(string error)
	{
		return new Result<T?>(false, default, [error]);
	}

	public static implicit operator Result<T>(T value) => Success(value);
}