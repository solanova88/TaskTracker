namespace TaskTracker.Application.Interfaces.Auth;

public interface IJwtService
{
	string GenerateJwt(string userName);
}