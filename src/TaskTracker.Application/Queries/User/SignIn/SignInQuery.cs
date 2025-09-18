using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Application.Queries.User.SignIn;

public record SignInQuery(string UserName) : IRequest<SignInResult>;