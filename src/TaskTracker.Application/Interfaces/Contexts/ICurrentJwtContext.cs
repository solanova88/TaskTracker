namespace TaskTracker.Application.Interfaces.Contexts;

public interface ICurrentJwtContext
{
	string? Jwt { get; set; }
}