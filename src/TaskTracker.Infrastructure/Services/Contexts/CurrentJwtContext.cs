using TaskTracker.Application.Interfaces.Contexts;

namespace TaskTracker.Infrastructure.Services.Contexts;

public class CurrentJwtContext : ICurrentJwtContext
{
	public string? Jwt { get; set; }
}