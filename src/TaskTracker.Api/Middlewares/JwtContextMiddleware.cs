using TaskTracker.Application.Interfaces.Contexts;

namespace TaskTracker.Api.Middlewares;

public class JwtContextMiddleware
{
	private readonly RequestDelegate _next;

	public JwtContextMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, ICurrentJwtContext jwtContext)
	{
		if (context.Request.Cookies.TryGetValue("access_token", out var jwt))
		{
			jwtContext.Jwt = jwt;
		}

		await _next(context);
	}
}