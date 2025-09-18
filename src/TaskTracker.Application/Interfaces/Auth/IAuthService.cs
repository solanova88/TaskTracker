using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.Queries.User.SignIn;

namespace TaskTracker.Application.Interfaces.Auth;

public interface IAuthService
{
	Task<SignInResult> SignInAsync(SignInQuery query);
}