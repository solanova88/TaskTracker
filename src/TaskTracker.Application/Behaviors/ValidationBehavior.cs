using FluentValidation;
using MediatR;
using TaskTracker.Application.Common.Models;

namespace TaskTracker.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (!_validators.Any()) return await next();

		var context = new ValidationContext<TRequest>(request);
		var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
			.SelectMany(r => r.Errors)
			.Where(f => f != null)
			.Select(f => f.ErrorMessage)
			.ToList();

		if (failures.Count == 0) return await next();
		
		if (!typeof(TResponse).IsGenericType ||
		    typeof(TResponse).GetGenericTypeDefinition() != typeof(Result<>))
			throw new ValidationException(failures.Select(f => new FluentValidation.Results.ValidationFailure("", f)));
		var valueType = typeof(TResponse).GetGenericArguments()[0];
		var failureMethod = typeof(Result<>)
			.MakeGenericType(valueType)
			.GetMethod(nameof(Result<int>.Failure), [typeof(IEnumerable<string>)]);
		if (failureMethod == null)
			throw new InvalidOperationException("Cannot find Failure method on Result<T>");
		return (TResponse)failureMethod.Invoke(null, [failures])!;
	}
}