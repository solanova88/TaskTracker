using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Queries.User.SignIn;

namespace TaskTracker.Application.Handlers.User.SignIn;

public class SignInQueryHandler :  IRequestHandler<SignInQuery, SignInResult>
{
	private readonly IAuthService _authService;

	public SignInQueryHandler(IAuthService authService)
	{
		_authService = authService;
	}

	public async Task<SignInResult> Handle(SignInQuery query, CancellationToken cancellationToken)
	{
		var result = await _authService.SignInAsync(query);
		return result;
	}
}