using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Commands.Tasks;
using TaskTracker.Application.Queries.Tasks;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/task")]
public class TaskController : ControllerBase
{
	private readonly ISender _mediator;

	public TaskController(ISender mediator)
	{
		_mediator = mediator;
	}

	[Authorize]
	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] CreateWorkTaskCommand request)
	{
		var result = await _mediator.Send(request);
		return result.Succeeded ? Ok(result) : BadRequest(result);
	}
	
	[Authorize]
	[HttpGet("get")]
	public async Task<IActionResult> Get([FromQuery] GetWorkTaskQuery query)
	{
		var result = await _mediator.Send(query);
		return result.Succeeded ? Ok(result) : BadRequest(result);
	}
	
	[Authorize]
	[HttpGet("get-list")]
	public async Task<IActionResult> Get([FromQuery] GetWorkTaskListQuery query)
	{
		var result = await _mediator.Send(query);
		return result.Succeeded ? Ok(result) : BadRequest(result);
	}
	
	[Authorize]
	[HttpPost("update")]
	public async Task<IActionResult> Update([FromBody] UpdateWorkTaskCommand request)
	{
		var result = await _mediator.Send(request);
		return result.Succeeded ? Ok(result) : BadRequest(result);
	}
	
	[Authorize]
	[HttpPost("delete")]
	public async Task<IActionResult> Delete([FromBody] DeleteWorkTaskCommand request)
	{
		var result = await _mediator.Send(request);
		return result.Succeeded ? Ok(result) : BadRequest(result);
	}
}