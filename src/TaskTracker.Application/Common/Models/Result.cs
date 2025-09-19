namespace TaskTracker.Application.Common.Models;

public class Result<T>
{
	private Result(bool succeeded, T value, IEnumerable<string> errors)
	{
		Succeeded = succeeded;
		Value = value;
		Errors = errors.ToArray();
	}

	public T Value { get; }
	public bool Succeeded { get; }
	public bool IsFailure => !Succeeded;
	public IReadOnlyCollection<string> Errors { get; }

	public static Result<T> Success(T value) =>
		new(true, value, []);

	public static Result<T> Failure(IEnumerable<string> errors) =>
		new(false, default, errors);

	public static Result<T> Failure(string error) =>
		new(false, default, new[] { error });

	public static implicit operator Result<T>(T value) => Success(value);
}