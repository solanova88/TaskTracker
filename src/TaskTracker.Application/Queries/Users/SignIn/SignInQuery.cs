using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Application.Queries.Users.SignIn;

public record SignInQuery(string UserName) : IRequest<SignInResult>;