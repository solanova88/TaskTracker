using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.Cookies;
using TaskTracker.Application.Queries.Users.SignIn;

namespace TaskTracker.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
	private readonly ICookieManagementService _cookieManagementService;

	public AuthService(ICookieManagementService cookieManagementService)
	{
		_cookieManagementService = cookieManagementService;
	}

	public Task<SignInResult> SignInAsync(SignInQuery query)
	{
		var result = _cookieManagementService.AppendCookie(query.UserName);
		
		return Task.FromResult(!result ? SignInResult.Failed : SignInResult.Success);
	}
}