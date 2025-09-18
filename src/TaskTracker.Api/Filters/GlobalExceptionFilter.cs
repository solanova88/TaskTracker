using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskTracker.Api.Filters;

public sealed class GlobalExceptionFilter : IExceptionFilter
{
	private readonly IWebHostEnvironment _env;

	public GlobalExceptionFilter(IWebHostEnvironment env)
	{
		_env = env;
	}

	public void OnException(ExceptionContext context)
	{
		var statusCode = DetermineStatusCode(context);

		var problemDetails = CreateProblemDetails(context, statusCode);

		context.Result = new ObjectResult(problemDetails)
		{
			StatusCode = (int)statusCode
		};

		context.ExceptionHandled = true;
	}

	private ProblemDetails CreateProblemDetails(ExceptionContext context, HttpStatusCode statusCode)
	{
		return new ProblemDetails
		{
			Status = (int)statusCode,
			Title = "An error occurred while processing your request.",
			Detail = context.Exception switch
			{
				_ => _env.IsDevelopment() ? context.Exception.ToString() : "A server error occurred."
			},
			Instance = context.HttpContext.Request.Path
		};
	}
	
	private static HttpStatusCode DetermineStatusCode(ExceptionContext context)
	{
		return context.Exception switch
		{
			ArgumentException => HttpStatusCode.BadRequest,
			UnauthorizedAccessException => HttpStatusCode.Unauthorized,
			_ => HttpStatusCode.InternalServerError
		};
	}
}