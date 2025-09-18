using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Queries.User.SignIn;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
	private readonly ISender _mediator;

	public UserController(ISender mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] SignInQuery command)
	{
		var result = await _mediator.Send(command);
		return result.Succeeded ? Ok(result) : Unauthorized(result);
	}
}