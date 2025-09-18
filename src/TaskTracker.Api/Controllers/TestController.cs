using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController :  ControllerBase
{
	[HttpGet("get-set")]
	public IActionResult PatchDraftOrder()
	{
		return Ok("ЗБС");
	}
}